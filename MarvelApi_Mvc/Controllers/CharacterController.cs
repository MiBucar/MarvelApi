using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Character;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MarvelApi_Mvc.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        public async Task<ActionResult> IndexCharacters(string searchQuery, int page = 1, int pageSize = 20)
        {
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            var allCharacters = new List<CharacterDTO>();
            var characters = new List<CharacterDTO>();

            var apiResponse = await _characterService.SearchAsync<ApiResponse>(searchQuery);
            if (apiResponse != null)
            {
                allCharacters = JsonConvert.DeserializeObject<IEnumerable<CharacterDTO>>(Convert.ToString(apiResponse.Result)).ToList();
            }

            if (allCharacters.Any())
                characters = allCharacters.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(allCharacters.Count() / (double)pageSize);

            var viewModel = new DisplayCharactersViewModel
            {
                CharacterDTOs = characters,
                TotalPages = totalPages,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<ActionResult> IndexCharacter(int id)
        {
            DisplayCharacterViewModel characterVM = new DisplayCharacterViewModel();

			var characterTask = _characterService.GetAsync<ApiResponse>(id);
			var alliesTask = _characterService.GetAlliesAsync<ApiResponse>(id);
			var enemiesTask = _characterService.GetEnemiesAsync<ApiResponse>(id);
			var teamTask = _characterService.GetTeamAsync<ApiResponse>(id);

			await Task.WhenAll(characterTask, alliesTask, enemiesTask, teamTask);

			var characterResponse = await characterTask;
			var alliesResponse = await alliesTask;
			var enemiesResponse = await enemiesTask;
			var teamResponse = await teamTask;

			if (characterResponse != null && characterResponse.IsSuccess)
			{
				characterVM.Character = JsonConvert.DeserializeObject<CharacterDTO>(Convert.ToString(characterResponse.Result));
			}

			if (alliesResponse != null && alliesResponse.IsSuccess)
			{
				characterVM.Allies = JsonConvert.DeserializeObject<List<CharacterDTO>>(Convert.ToString(alliesResponse.Result));
			}

			if (enemiesResponse != null && enemiesResponse.IsSuccess)
			{
				characterVM.Enemies = JsonConvert.DeserializeObject<List<CharacterDTO>>(Convert.ToString(enemiesResponse.Result));
			}

			if (teamResponse != null && teamResponse.IsSuccess)
			{
				characterVM.Team = JsonConvert.DeserializeObject<TeamDTO>(Convert.ToString(teamResponse.Result));
			}

			return View(characterVM);
		}		
    }
}
