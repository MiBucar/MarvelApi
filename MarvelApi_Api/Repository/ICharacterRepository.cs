using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using MarvelApi_Api.Models.DTOs.Character;
using MarvelApi_Api.Repository;

namespace MarvelApi_Api.Repository
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Task<Character> UpdateAsync(CharacterUpdateDTO characterDto);
        Task<Character> AddEnemyAsync(int characterId, List<int> enemyIds);
        Task<Character> AddAllyAsync(int characterId, List<int> allyIds);
    }
}