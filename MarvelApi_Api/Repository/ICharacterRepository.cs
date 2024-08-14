using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Repository;

namespace MarvelApi_Api.Repository
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Task<Character> UpdateAsync(CharacterUpdateDTO characterDto);
        Task<Character> AssignEnemiesAsync(int characterId, List<int> enemyIds);
        Task<Character> AssignAlliesAsync(int characterId, List<int> allyIds);
        Task<Character> AssignTeamToCharacterAsync(int teamId, int characterId);
		Task AssignTeamToCharactersAsync(int teamId, List<int> characterIds);
		Task<IEnumerable<Character>> GetAlliesAsync(int id);
        Task<IEnumerable<Character>> GetEnemiesAsync(int id);
        Task<Team> GetTeamAsync(int id);
    }
}