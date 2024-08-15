using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Home;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MarvelApi_Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICharacterService _characterService;
        private readonly ITeamService _teamService;
        public HomeController(ICharacterService characterService, ITeamService teamService, ILogger<HomeController> logger)
        {
            _characterService = characterService;
            _teamService = teamService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new();
            var characters = new List<CharacterDTO>();
            var apiResponse = await _characterService.GetAllAsync<ApiResponse>();
            if (apiResponse != null && apiResponse.IsSuccess)
            {
                characters = JsonConvert.DeserializeObject<IEnumerable<CharacterDTO>>(Convert.ToString(apiResponse.Result)).ToList();
            }
            var teams = new List<TeamDTO>();
            var response = await _teamService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess)
            {
                teams = JsonConvert.DeserializeObject<IEnumerable<TeamDTO>>(Convert.ToString(response.Result)).ToList();
            }

            homeViewModel.CharacterDTOs = characters;
            homeViewModel.TeamDTOs = teams;
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
