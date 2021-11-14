using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Context
{
    public class AuthDbContext : IdentityDbContext<UserBase>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ModelConfig(builder);
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProccessAuditing();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ModelConfig(ModelBuilder builder)
        {
            builder.Entity<UserClient>(
                eb =>
                {
                    eb.HasKey(e => e.Id);
                    eb.Property(e => e.Height).HasColumnType("decimal(6,2)").IsRequired();
                    eb.Property(e => e.Phone).HasColumnType("nvarchar(13)").IsRequired();
                    eb.HasOne(e => e.User).WithOne();
                });
        }

        private void ProccessAuditing()
        {
            var currentDate = DateTimeOffset.Now;
            
            foreach (var item in ChangeTracker.Entries().Where(e => 
                                                            e.State == EntityState.Added && 
                                                            e.Entity is Auditable))
            {
                var entity = item.Entity as Auditable;
                entity.CreatedBy = "system";
                entity.CreatedDate = currentDate;
            }

            foreach (var item in ChangeTracker.Entries().Where(e =>
                                                            e.State == EntityState.Modified &&
                                                            e.Entity is Auditable))
            {
                var entity = item.Entity as Auditable;
                entity.ModifiedBy = "system";
                entity.ModifiedDate = currentDate;
                item.Property(nameof(entity.CreatedDate)).IsModified = false;
                item.Property(nameof(entity.CreatedBy)).IsModified = false;
            }

        }

        public DbSet<UserClient> UserClients { get; set; }
    }
}
