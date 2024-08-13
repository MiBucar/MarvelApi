using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.CharacterDTOS;
using MarvelApi_Api.Models.DTOs.Team;

namespace MarvelApi_Api.Repository
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> UpdateAsync(TeamUpdateDTO teamUpdateDTO);
        Task<IEnumerable<Character>> GetMembersAsync(int id);
    }
}