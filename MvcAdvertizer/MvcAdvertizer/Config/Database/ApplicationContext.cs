using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Config.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAdvertsCounter> UsersAdvertsCounters { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)        
        {
                       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserAdvertsCounter>()
                .Property(b => b.Count)
                .IsConcurrencyToken();            
            modelBuilder.Entity<UserAdvertsCounter>()
                .HasIndex(b => b.UserId)
                .IsUnique();
        }

        public override int SaveChanges()
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditedEntity>()
            .Where(p => p.State == EntityState.Added)
            .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditedEntity>()
              .Where(p => p.State == EntityState.Modified)
              .Select(p => p.Entity);

            var now = DateTime.UtcNow;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedOn = now;
                added.UpdatedOn = now;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.UpdatedOn = now;
            }

            return base.SaveChanges();
        }
    }
}
