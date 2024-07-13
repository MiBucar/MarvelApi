using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.ViewModels.Character;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MarvelApi_Mvc.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly ITeamService _teamService;
        private readonly ISelectListItemGetters _selectListItemGetters;
        public CharacterController(ICharacterService characterService, ITeamService teamService, ISelectListItemGetters selectListItemGetters)
        {
            _characterService = characterService;
            _teamService = teamService;
            _selectListItemGetters = selectListItemGetters;
        }

        public async Task<ActionResult> IndexCharacter()
        {
            var apiResponse = await _characterService.GetAllAsync<ApiResponse>();
            if (apiResponse != null)
            {
                var characters = JsonConvert.DeserializeObject<IEnumerable<CharacterDTO>>(Convert.ToString(apiResponse.Result));
                return View(characters);
            }
            return View();
        }

        public async Task<ActionResult> CreateCharacter()
        {
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
            var availableTeams = await _selectListItemGetters.GetAvailableTeamsAsync();

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

                var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
                var availableTeams = await _selectListItemGetters.GetAvailableTeamsAsync();
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
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
            var availableTeams = await _selectListItemGetters.GetAvailableTeamsAsync();
            var characterForUpdate = await _selectListItemGetters.GetCharacterAsync(id);

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

                var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
                var availableTeams = await _selectListItemGetters.GetAvailableTeamsAsync();
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
    }
}
