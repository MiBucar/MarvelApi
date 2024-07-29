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
        public DbSet<User> Users { get; set; }

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
        }
    }
}