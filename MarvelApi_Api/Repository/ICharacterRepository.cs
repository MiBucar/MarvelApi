using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Repository;

namespace MarvelApi_Api.Repository
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Task<Character> UpdateAsync(CharacterUpdateDTO characterDto);
        Task<Character> AddEnemyAsync(int characterId, List<int> enemyIds);
        Task<Character> AddAllyAsync(int characterId, List<int> allyIds);
        Task<Character> AddTeamToCharacter(int teamId, int characterId);
        Task<IEnumerable<Character>> GetAllies(int id);
        Task<IEnumerable<Character>> GetEnemies(int id);
        Task<Team> GetTeam(int id);
        Task AddTeamsToCharacter(int teamId, List<int> characterIds);
    }
}