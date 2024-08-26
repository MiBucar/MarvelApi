using AutoMapper;
using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Character;
using MarvelApi_Mvc.Models.ViewModels.Team;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MarvelApi_Mvc.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly ITeamService _teamService;
        private readonly ISelectListItemGetters _selectListItemGetters;
        private readonly IMapper _autoMapper;

        public AdminDashboardController(ICharacterService character, ITeamService teamService, ISelectListItemGetters selectListItemGetters, IMapper autoMapper)
        {
            _teamService = teamService;
            _characterService = character;
            _selectListItemGetters = selectListItemGetters;
            _autoMapper = autoMapper;
        }

        public async Task<ActionResult> IndexAdminDashboard(){
            return View();
        }

        public async Task<ActionResult> IndexDashboardCharacters(string searchQuery)
        {
            CharactersManagementViewModel charactersManagementViewModel = new CharactersManagementViewModel();
            var apiResponse = await _characterService.SearchAsync<ApiResponse>(searchQuery);
            if (apiResponse != null && apiResponse.IsSuccess)
            {
                charactersManagementViewModel.Characters = JsonConvert.DeserializeObject<IEnumerable<CharacterDTO>>(Convert.ToString(apiResponse.Result));
                return View(charactersManagementViewModel);
            }
            return View();
        }

        public async Task<ActionResult> IndexDashboardTeams(string searchQuery)
        {
            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            var apiResponse = await _teamService.SearchAsync<ApiResponse>(searchQuery);
            if (apiResponse != null && apiResponse.IsSuccess)
            {
                teamManagementViewModel.Teams = JsonConvert.DeserializeObject<IEnumerable<TeamDTO>>(Convert.ToString(apiResponse.Result));
                return View(teamManagementViewModel);
            }
            return View();
        }

        [Authorize(Roles = "Admin,User")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateCharacter(CharacterCreateViewModel characterCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _characterService.CreateAsync<ApiResponse>(characterCreateViewModel.CharacterCreateDTO);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(IndexDashboardCharacters));
                    }
                    else if (response == null)
                    {
                        ModelState.AddModelError(string.Empty, "Only admin is allowed to make this action");
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

        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> UpdateCharacter(int id)
        {
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
            var availableTeams = await _selectListItemGetters.GetAvailableTeamsAsync();
            var characterDTO = await _selectListItemGetters.GetCharacterAsync(id);

            var characterUpdateDTO = _autoMapper.Map<CharacterUpdateDTO>(characterDTO);

            CharacterUpdateViewModel characterUpdateVM = new CharacterUpdateViewModel();
            characterUpdateVM.CharacterUpdateDTO = characterUpdateDTO;
            characterUpdateVM.AvailableCharacters = availableCharacters;
            characterUpdateVM.AvailableTeams = availableTeams;
            return View(characterUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCharacter(CharacterUpdateViewModel characterUpdateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (characterUpdateViewModel.CharacterUpdateDTO.ImageFile != null)
                    //{
                    //    using (var memoryStream = new MemoryStream())
                    //    {
                    //        await characterUpdateViewModel.CharacterUpdateDTO.ImageFile.CopyToAsync(memoryStream);
                    //        characterUpdateViewModel.CharacterUpdateDTO.Image = memoryStream.ToArray();
                    //    }
                    //}

                    var response = await _characterService.UpdateAsync<ApiResponse>(characterUpdateViewModel.CharacterUpdateDTO);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(IndexDashboardCharacters));
                    }
                    else if (response == null)
                    {
                        ModelState.AddModelError(string.Empty, "Only admin is allowed to make this action");
                    }
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

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteAsync<ApiResponse>(id);
            return RedirectToAction(nameof(IndexDashboardCharacters));
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> CreateTeam()
        {
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();

            var teamCreateViewModel = new CreateTeamViewModel();
            TeamCreateDTO teamCreateDTO = new TeamCreateDTO();
            teamCreateViewModel.AvailableCharacters = availableCharacters;
            teamCreateViewModel.TeamCreateDTO = teamCreateDTO;
            return View(teamCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTeam(CreateTeamViewModel teamCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _teamService.CreateAsync<ApiResponse>(teamCreateViewModel.TeamCreateDTO);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(IndexDashboardTeams));
                    }
                    else if (response == null)
                    {
                        ModelState.AddModelError(string.Empty, "Only admin is allowed to make this action");
                    }
                }

                var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
                teamCreateViewModel.AvailableCharacters = availableCharacters;
                return View(teamCreateViewModel);
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateTeam(int id)
        {
            var teamDTO = await _selectListItemGetters.GetTeamAsync(id);
            var teamUpdateDTO = _autoMapper.Map<TeamUpdateDTO>(teamDTO);
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();

            UpdateTeamViewModel teamUpdateViewModel = new UpdateTeamViewModel();
            teamUpdateViewModel.TeamUpdateDTO = teamUpdateDTO;
            teamUpdateViewModel.AvailableCharacters = availableCharacters;
            return View(teamUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> UpdateTeam(UpdateTeamViewModel teamUpdateViewModel)
        {
            try
            {
                var response = await _teamService.UpdateAsync<ApiResponse>(teamUpdateViewModel.TeamUpdateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexDashboardTeams));
                }
                else if (response == null)
                {
                    ModelState.AddModelError(string.Empty, "Only admin is allowed to make this action");
                }

                var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();
                teamUpdateViewModel.AvailableCharacters = availableCharacters;
                return View(teamUpdateViewModel);
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var response = await _teamService.DeleteAsync<ApiResponse>(id);
            return RedirectToAction(nameof(IndexDashboardTeams));
        }
    }
}
