using System.Data.Common;
using MarvelApi_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterRelationship> CharacterRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CharacterRelationship>()
            .HasKey(cr => new { cr.CharacterId, cr.RelatedCharacterId });

            modelBuilder.Entity<CharacterRelationship>()
                .HasOne(cr => cr.Character)
                .WithMany(c => c.CharacterRelationships)
                .HasForeignKey(cr => cr.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CharacterRelationship>()
                .HasOne(cr => cr.RelatedCharacter)
                .WithMany()
                .HasForeignKey(cr => cr.RelatedCharacterId)
                .OnDelete(DeleteBehavior.Restrict);

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
            
            modelBuilder.Entity<Team>().HasData(
                new Team{
                    Id = 1,
                    Name = "Avengers",
                    Description = "Hero team.",
                },
                new Team{
                    Id = 2,
                    Name = "Children of Thanos",
                    Description = "Team trying to destroy the world.",
                }
            );
        }
    }
}