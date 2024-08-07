﻿using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Team;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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

        public async Task<ActionResult> IndexTeam(string searchQuery, int page = 1, int pageSize = 20)
        {
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            var allTeams = new List<TeamDTO>();
            var teams = new List<TeamDTO>();

            var apiResponse = await _teamService.SearchAsync<ApiResponse>(searchQuery);
            if (apiResponse != null && apiResponse.IsSuccess)
            {
                allTeams = JsonConvert.DeserializeObject<IEnumerable<TeamDTO>>(Convert.ToString(apiResponse.Result)).ToList();
            }

            if (allTeams.Any())
                teams = allTeams.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(allTeams.Count() / (double)pageSize);

            var viewModel = new DisplayTeamsViewModel
            {
                TeamDTOs = teams,
                CurrentPage = page,
                TotalPages = totalPages,
            };

            return View(viewModel);
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
                    if (teamCreateViewModel.TeamCreateDTO.ImageFile != null){
                        using (var memoryStream = new MemoryStream()){
                            await teamCreateViewModel.TeamCreateDTO.ImageFile.CopyToAsync(memoryStream);
                            teamCreateViewModel.TeamCreateDTO.Image = memoryStream.ToArray();
                        }
                    }

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
                if (teamUpdateViewModel.TeamUpdateDTO.ImageFile != null){
                        using (var memoryStream = new MemoryStream()){
                            await teamUpdateViewModel.TeamUpdateDTO.ImageFile.CopyToAsync(memoryStream);
                            teamUpdateViewModel.TeamUpdateDTO.Image = memoryStream.ToArray();
                        }
                    }

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
