using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Team;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MarvelApi_Mvc.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ICharacterService _characterService;
        private readonly ISelectListItemGetters _selectListItemGetters;

        public TeamController(ITeamService teamService, ICharacterService characterService, ISelectListItemGetters selectListItemGetters)
        {
            _teamService = teamService;
            _characterService = characterService;
            _selectListItemGetters = selectListItemGetters;
        }

        public async Task<ActionResult> IndexTeam()
        {
            var teams = new List<TeamDTO>();
            var response = await _teamService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess)
            {
                teams = JsonConvert.DeserializeObject<IEnumerable<TeamDTO>>(Convert.ToString(response.Result)).ToList();
            }
            return View(teams);
        }

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
        public async Task<ActionResult> CreateTeam(TeamCreateViewModel teamCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _teamService.CreateAsync<ApiResponse>(teamCreateViewModel.TeamCreateDTO);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(IndexTeam));
                    }
                    else if (response == null){
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
        public async Task<ActionResult> UpdateTeam(TeamUpdateViewModel teamUpdateViewModel)
        {
            try
            {
                var response = await _teamService.UpdateAsync<ApiResponse>(teamUpdateViewModel.TeamUpdateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexTeam));
                }
                else if (response == null){
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

        public async Task<ActionResult> DeleteTeam(int id)
        {
            var response = await _teamService.DeleteAsync<ApiResponse>(id);
            return RedirectToAction(nameof(IndexTeam));
        }
    }
}
