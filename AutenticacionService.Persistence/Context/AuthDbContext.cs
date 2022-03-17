using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
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
                    eb.HasOne(e => e.User).WithOne().IsRequired();
                });

            builder.Entity<UserAtelier>(
                eb =>
                {
                    eb.HasKey(e => e.Id);
                    eb.Property(e => e.Role).HasColumnType("nvarchar(20)").IsRequired();
                    eb.Property(e => e.Dni).HasColumnType("nvarchar(8)").IsRequired();
                    eb.Property(e => e.BossId).HasColumnType("int").IsRequired(false);
                    eb.HasOne(e => e.User).WithOne().IsRequired();
                    eb.HasOne(e => e.Atelier).WithMany(atl => atl.Employees).IsRequired();
                });

            builder.Entity<Atelier>(
                eb =>
                {
                    eb.HasKey(e => e.Id);
                    eb.Property(e => e.NameAtelier).HasColumnType("nvarchar(100)").IsRequired();
                    eb.Property(e => e.RucAtelier).HasColumnType("nvarchar(11)").IsRequired();
                    eb.Property(e => e.City).HasColumnType("nvarchar(100)").IsRequired();
                    eb.Property(e => e.District).HasColumnType("nvarchar(100)").IsRequired();
                    eb.Property(e => e.Address).HasColumnType("nvarchar(max)").IsRequired();
                    eb.Property(e => e.DescriptionAtelier).HasColumnType("nvarchar(max)").IsRequired();
                    eb.HasMany(e => e.Employees).WithOne(empl => empl.Atelier).OnDelete(DeleteBehavior.ClientCascade);
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
                entity.Status = StatusUtil.ACTIVE;
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
        public DbSet<UserAtelier> UserAteliers { get; set; }
        public DbSet<Atelier> Ateliers { get; set; }
    }
}
