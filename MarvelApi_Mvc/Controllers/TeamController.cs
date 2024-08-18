using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Team;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace MarvelApi_Mvc.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<ActionResult> IndexTeams(string searchQuery, int page = 1, int pageSize = 20)
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

        public async Task<ActionResult> IndexTeam(int id)
        {
            DisplayTeamViewModel teamVM = new DisplayTeamViewModel();
            var apiResponse = await _teamService.GetAsync<ApiResponse>(id);
            if (apiResponse != null && apiResponse.IsSuccess)
            {
                var existingTeam = JsonConvert.DeserializeObject<TeamDTO>(Convert.ToString(apiResponse.Result));
                teamVM.Team = existingTeam;
            }

            apiResponse = await _teamService.GetMembersAsync<ApiResponse>(id);
			if (apiResponse != null && apiResponse.IsSuccess)
			{
				var existingMembers = JsonConvert.DeserializeObject<List<CharacterDTO>>(Convert.ToString(apiResponse.Result));
				teamVM.Members = existingMembers;
			}
			return View(teamVM);
        }
    }
}
