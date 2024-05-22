using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Repository.Implementation
{
    public class HeroRepository : Repository<Hero>, IHeroRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HeroRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Hero> UpdateAsync(Hero hero)
        {
            hero.DateUpdated = DateTime.UtcNow;
            _dbContext.Update(hero);
            await _dbContext.SaveChangesAsync();
            return hero;
        }
    }
}