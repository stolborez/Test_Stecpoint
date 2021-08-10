using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Stecpoint.Data.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("CDC084D3-2108-4D05-A10F-147448918690"),
                    Email = "test@mail.ru",
                    Number = "3333",
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович"
                }
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = new Guid("DDC084D3-2108-4D05-A10F-147448918690"),
                    Name = "Test Organization"
                }
            );

            base.OnModelCreating(modelBuilder);
        }

    }

    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>().UseNpgsql(args[0]);
            return new DataContext(optionsBuilder.Options);
        }
    }
}
