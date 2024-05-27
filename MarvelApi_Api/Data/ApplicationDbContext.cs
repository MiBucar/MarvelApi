using System.Data.Common;
using MarvelApi_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Character> Characters { get; set; }
        // public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                        .HasMany(c => c.Allies)
                        .WithMany()
                        .UsingEntity<Dictionary<string, object>>(
                            "CharacterAllies",
                            j => j
                                .HasOne<Character>()
                                .WithMany()
                                .HasForeignKey("AllyId")
                                .OnDelete(DeleteBehavior.Cascade),
                            j => j
                                .HasOne<Character>()
                                .WithMany()
                                .HasForeignKey("CharacterId")
                                .OnDelete(DeleteBehavior.Cascade)
                        );

            modelBuilder.Entity<Character>()
                        .HasMany(c => c.Enemies)
                        .WithMany()
                        .UsingEntity<Dictionary<string, object>>(
                            "CharacterEnemies",
                            j => j
                                .HasOne<Character>()
                                .WithMany()
                                .HasForeignKey("EnemyId")
                                .OnDelete(DeleteBehavior.Cascade),
                            j => j
                                .HasOne<Character>()
                                .WithMany()
                                .HasForeignKey("CharacterId")
                                .OnDelete(DeleteBehavior.Cascade)
                        );

            modelBuilder.Entity<Character>().HasData(
                new Character
                {
                    Id = 1,
                    Name = "Iron man",
                    IsVillain = false,
                    Backstory = "Was  rich man",
                    Appearance = "Avengers",
                    Origin = "New York",
                    Powers = new List<string> { "laser", "Big laser" }
                },
                new Character
                {
                    Id = 2,
                    Name = "Thanos",
                    IsVillain = true,
                    Backstory = "Was  rich man",
                    Appearance = "Avengers",
                    Origin = "New York",
                    Powers = new List<string> { "laser", "Big laser" }
                },
                new Character
                {
                    Id = 3,
                    Name = "Daredevil",
                    IsVillain = false,
                    Backstory = "Was  rich man",
                    Appearance = "Avengers",
                    Origin = "New York",
                    Powers = new List<string> { "laser", "Big laser" }
                }
            );
            
            // modelBuilder.Entity<Team>().HasData(
            //     new Team{
            //         Id = 1,
            //         Name = "Avengers",
            //         Description = "Hero team.",
            //     },
            //     new Team{
            //         Id = 2,
            //         Name = "Children of Thanos",
            //         Description = "Team trying to destroy the world.",
            //     }
            // );

            base.OnModelCreating(modelBuilder);
        }
    }
}