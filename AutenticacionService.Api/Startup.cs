using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutenticacionService.Api.ApiConventions;
using AutenticacionService.Business.Mapper;
using AutenticacionService.Business.ServicesCommand.Implements;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Repositories.Implements;
using AutenticacionService.Persistence.Repositories.Interfaces;
using AutenticacionService.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AutenticacionService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // db connection
            services.AddDbContext<AuthDbContext>(
                opts => opts.UseSqlServer(Configuration.GetConnectionString("LocalConnection"))
                );

            // ms identity
            services.AddIdentity<UserBase, IdentityRole>(
                opts =>
                {
                    opts.Password.RequiredLength = 8;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            // repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserClientRepository, UserClientRepository>();

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // services
            services.AddScoped<IUserClientServiceCommand, UserClientServiceCommand>();

            // mapper
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MapperProfile())
                );
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    opts => opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"])),
                        ClockSkew = TimeSpan.Zero
                    }
                );

            // swagger doc
            services.AddSwaggerGen(
                config =>
                {
                    config.SwaggerDoc("v1", new OpenApiInfo 
                    {
                        Version = "v1",
                        Title = "API Autenticacion v1",
                        Description = "Servicio de Autenticacion del sistema Patronaje A Medida",
                        Contact = new OpenApiContact
                        {
                            Name = "Patronaje A Medida",
                            Email = "",
                            Url = new Uri("https://github.com/Patronaje-A-Medida")
                        }
                    });
                });

            // CORS
            services.AddCors(
                opts => opts.AddPolicy(
                    "All",
                    builder => builder.WithOrigins("*").WithHeaders("*").WithMethods("*"))
                );
                
            // controllers
            services.AddControllers(
                config => config.Conventions.Add(new ApiVersionConvention())
                )
                .AddNewtonsoftJson(
                    opts => opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            // configura error
            services.Configure<ApiBehaviorOptions>(
                options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "API Autenticacion v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("All");

            app.UseRouting();

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
