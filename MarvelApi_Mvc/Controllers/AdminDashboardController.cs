using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Character;
using MarvelApi_Mvc.Models.ViewModels.Team;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Mvc.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly ITeamService _teamService;
        private readonly ISelectListItemGetters _selectListItemGetters;

        public AdminDashboardController(ICharacterService character, ITeamService teamService, ISelectListItemGetters selectListItemGetters)
        {
            _teamService = teamService;
            _characterService = character;
            _selectListItemGetters = selectListItemGetters;

        }

        public async Task<ActionResult> IndexAdminDashboard(){
            return View();
        }

        public async Task<ActionResult> IndexDashboardCharacters()
        {
            return View();
        }

        public async Task<ActionResult> IndexDashboardTeams()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
                    if (characterCreateViewModel.CharacterCreateDTO.ImageFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await characterCreateViewModel.CharacterCreateDTO.ImageFile.CopyToAsync(memoryStream);
                            characterCreateViewModel.CharacterCreateDTO.Image = memoryStream.ToArray();
                        }
                    }

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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCharacter(CharacterUpdateViewModel characterUpdateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (characterUpdateViewModel.CharacterUpdateDTO.ImageFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await characterUpdateViewModel.CharacterUpdateDTO.ImageFile.CopyToAsync(memoryStream);
                            characterUpdateViewModel.CharacterUpdateDTO.Image = memoryStream.ToArray();
                        }
                    }

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

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTeam()
        {
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();

            var teamCreateViewModel = new TeamCreateViewModel();
            TeamCreateDTO teamCreateDTO = new TeamCreateDTO();
            teamCreateViewModel.AvailableCharacters = availableCharacters;
            teamCreateViewModel.TeamCreateDTO = teamCreateDTO;
            return View(teamCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTeam(TeamCreateViewModel teamCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teamCreateViewModel.TeamCreateDTO.ImageFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await teamCreateViewModel.TeamCreateDTO.ImageFile.CopyToAsync(memoryStream);
                            teamCreateViewModel.TeamCreateDTO.Image = memoryStream.ToArray();
                        }
                    }

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
            var team = await _selectListItemGetters.GetTeamAsync(id);
            var availableCharacters = await _selectListItemGetters.GetAvailableCharactersAsync();

            TeamUpdateViewModel teamUpdateViewModel = new TeamUpdateViewModel();
            teamUpdateViewModel.TeamUpdateDTO = team;
            teamUpdateViewModel.AvailableCharacters = availableCharacters;
            return View(teamUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateTeam(TeamUpdateViewModel teamUpdateViewModel)
        {
            try
            {
                if (teamUpdateViewModel.TeamUpdateDTO.ImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await teamUpdateViewModel.TeamUpdateDTO.ImageFile.CopyToAsync(memoryStream);
                        teamUpdateViewModel.TeamUpdateDTO.Image = memoryStream.ToArray();
                    }
                }

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
