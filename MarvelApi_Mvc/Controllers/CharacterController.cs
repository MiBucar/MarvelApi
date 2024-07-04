using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.ViewModels;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Http;
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

        public ActionResult CreateCharacter()
        {
            var characterCreateViewModel = new CharacterCreateViewModel();
            return View(characterCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCharacter(CharacterCreateViewModel viewModel, string action)
        {
            try
            {
                if (action == "addnewpower")
                {
                    viewModel.AddNewPower();
                    return View(viewModel);
                }
                if (action == "deletepower")
                {
                    viewModel.DeletePower();
                    return View(viewModel);
                }
                if (ModelState.IsValid)
                {
                    var characterCreateDto = viewModel.CharacterCreateDTO;
                    var response = await _characterService.CreateAsync<ApiResponse>(characterCreateDto);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(IndexCharacter));
                    }
                }
                return View(viewModel);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UpdateCharacter(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCharacter(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteCharacter(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCharacter(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
