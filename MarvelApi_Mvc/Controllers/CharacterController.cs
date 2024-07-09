using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace MarvelApi_Mvc.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly ITeamService _teamService;
        public CharacterController(ICharacterService characterService, ITeamService teamService)
        {
            _characterService = characterService;
            _teamService = teamService;
        }

        public async Task<ActionResult> IndexCharacter()
        {
            var apiResponse = await _characterService.GetAllAsync<ApiResponse>();
            if (apiResponse != null)
            {
                var characters = JsonConvert.DeserializeObject<IEnumerable<CharacterDTO>>(apiResponse.Result.ToString());
                return View(characters);
            }
            return View();
        }

        public async Task<ActionResult> CreateCharacter()
        {
            var availableCharacters = await GetAvailableCharacters();
            var availableTeams = await GetAvailableTeams();

            CharacterCreateViewModel characterCreateVM = new CharacterCreateViewModel();
            characterCreateVM.CharacterCreateDTO = new CharacterCreateDTO();
            characterCreateVM.AvailableCharacters = availableCharacters;
            characterCreateVM.AvailableTeams = availableTeams;
            return View(characterCreateVM);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCharacter(CharacterCreateViewModel characterCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _characterService.CreateAsync<ApiResponse>(characterCreateViewModel.CharacterCreateDTO);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(IndexCharacter));
                    }
                }

                var availableCharacters = await GetAvailableCharacters();
                var availableTeams = await GetAvailableTeams();
                characterCreateViewModel.AvailableCharacters = availableCharacters;
                characterCreateViewModel.AvailableTeams = availableTeams;
                return View(characterCreateViewModel);
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> UpdateCharacter(int id)
        {
            var availableCharacters = await GetAvailableCharacters();
            var availableTeams = await GetAvailableTeams();
            var characterForUpdate = await GetCharacterForUpdate(id);

            CharacterUpdateViewModel characterUpdateVM = new CharacterUpdateViewModel();
            characterUpdateVM.CharacterUpdateDTO = characterForUpdate;
            characterUpdateVM.AvailableCharacters = availableCharacters;
            characterUpdateVM.AvailableTeams = availableTeams;
            return View(characterUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCharacter(CharacterUpdateViewModel characterUpdateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _characterService.UpdateAsync<ApiResponse>(characterUpdateViewModel.CharacterUpdateDTO);
                    if (response.IsSuccess)
                        return RedirectToAction(nameof(IndexCharacter));
                }

                var availableCharacters = await GetAvailableCharacters();
                var availableTeams = await GetAvailableTeams();
                characterUpdateViewModel.AvailableTeams = availableTeams;
                characterUpdateViewModel.AvailableCharacters = availableCharacters;
                return View(characterUpdateViewModel);
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteAsync<ApiResponse>(id);
            return RedirectToAction(nameof(IndexCharacter));
        }

        private async Task<CharacterUpdateDTO> GetCharacterForUpdate(int id)
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

        private async Task<List<SelectListItem>> GetAvailableCharacters()
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

        private async Task<List<SelectListItem>> GetAvailableTeams()
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

                availableTeamsListItems.Insert(0,
                    new SelectListItem
                    {
                        Value = "",
                        Text = "Select a team"
                    }
                );

                return availableTeamsListItems;
            }
            return new List<SelectListItem>();
        }
    }
}
