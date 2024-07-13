using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarvelApi_Mvc.Services.IServices
{
    public interface ISelectListItemGetters
    {
        Task<List<SelectListItem>> GetAvailableCharactersAsync();
        Task<List<SelectListItem>> GetAvailableTeamsAsync();
        Task<CharacterUpdateDTO> GetCharacterAsync(int id);
        Task<TeamUpdateDTO> GetTeamAsync(int id);
    }
}
