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

        public async Task<CharacterUpdateDTO> GetCharacterAsync(int id)
        {
            try
            {
                var response = await _characterService.GetAsync<ApiResponse>(id);
                if (response != null && response.IsSuccess)
                {
                    var character = JsonConvert.DeserializeObject<CharacterUpdateDTO>(Convert.ToString(response.Result));
                    return character;
                }

                return new CharacterUpdateDTO();
            }
            catch (Exception e)
            {
                return new CharacterUpdateDTO();
            }
        }

        public async Task<TeamUpdateDTO> GetTeamAsync(int id)
        {
            try
            {
                var response = await _teamService.GetAsync<ApiResponse>(id);
                if (response != null && response.IsSuccess)
                {
                    var team = JsonConvert.DeserializeObject<TeamUpdateDTO>(Convert.ToString(response.Result));
                    return team;
                }

                return new TeamUpdateDTO();
            }
            catch (Exception e)
            {
                return new TeamUpdateDTO();
            }
        }
    }
}
