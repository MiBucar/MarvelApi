using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using MarvelApi_Api.Repository;

namespace MarvelApi_Api.Repository
{
    public interface IHeroRepository : IRepository<Hero>
    {
        Task<Hero> UpdateAsync(Hero hero);
    }
}