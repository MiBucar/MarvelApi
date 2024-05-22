using System.Data.Common;
using MarvelApi_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Hero> Heroes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hero>().HasData(
                new Hero{
                    Id = 1,
                    Name = "Iron man",
                    IsVillain = false,
                },
                new Hero{
                    Id = 2,
                    Name = "Thanos",
                    IsVillain = true,
                },
                new Hero{
                    Id = 3,
                    Name = "Daredevil",
                    IsVillain = false,
                }
            );
        }
    }
}