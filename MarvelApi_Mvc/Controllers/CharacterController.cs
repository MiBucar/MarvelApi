using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.ViewModels.Character;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
                        return RedirectToAction(nameof(IndexCharacters));
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
                        return RedirectToAction(nameof(IndexCharacters));
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
            return RedirectToAction(nameof(IndexCharacters));
        }
    }
}
