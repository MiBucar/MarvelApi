using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MarvelApi_Mvc.Services.Implementation
{
    public class SelectListItemGetters : ISelectListItemGetters
    {
        private readonly ICharacterService _characterService;
        private readonly ITeamService _teamService;

        public SelectListItemGetters(ICharacterService characterService, ITeamService teamService)
        {
            _characterService = characterService;
            _teamService = teamService;
        }

        public async Task<List<SelectListItem>> GetAvailableCharactersAsync()
        {
            var response = await _characterService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess)
            {
                var availableCharacters = JsonConvert.DeserializeObject<List<CharacterDTO>>(Convert.ToString(response.Result));
                var availableCharactersListItems = availableCharacters.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToList();
                return availableCharactersListItems;
            }
            return new List<SelectListItem>();
        }

        public async Task<List<SelectListItem>> GetAvailableTeamsAsync()
        {
            var response = await _teamService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess)
            {
                var availableTeams = JsonConvert.DeserializeObject<List<TeamDTO>>(Convert.ToString(response.Result));
                var availableTeamsListItems = availableTeams.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToList();

                return availableTeamsListItems;
            }
            return new List<SelectListItem>();
        }

        public async Task<CharacterDTO> GetCharacterAsync(int id)
        {
            try
            {
                var response = await _characterService.GetAsync<ApiResponse>(id);
                if (response != null && response.IsSuccess)
                {
                    var character = JsonConvert.DeserializeObject<CharacterDTO>(Convert.ToString(response.Result));
                    return character;
                }

                return new CharacterDTO();
            }
            catch (Exception e)
            {
                return new CharacterDTO();
            }
        }

        public async Task<TeamDTO> GetTeamAsync(int id)
        {
            try
            {
                var response = await _teamService.GetAsync<ApiResponse>(id);
                if (response != null && response.IsSuccess)
                {
                    var team = JsonConvert.DeserializeObject<TeamDTO>(Convert.ToString(response.Result));
                    return team;
                }

                return new TeamDTO();
            }
            catch (Exception e)
            {
                return new TeamDTO();
            }
        }
    }
}
