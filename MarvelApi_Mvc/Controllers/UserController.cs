using System.ComponentModel;
using System.Text.Json;
using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.UserDTOs;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace MarvelApi_Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Login(){
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDTO loginRequestDTO){
            if (ModelState.IsValid){
                var response = await _userService.LoginAsync<ApiResponse>(loginRequestDTO);
                if (response.Result != null && response.IsSuccess){
                    var dataObject = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                    HttpContext.Session.SetString("JWToken", dataObject.Token);
                    return RedirectToAction("Index", "Home");
                }
                else if (response != null && !response.IsSuccess){
                    ModelState.AddModelError(string.Empty, response.ErrorMessages.FirstOrDefault());
                }
            }
            return View(loginRequestDTO);
        }

        public async Task<ActionResult> Register(){
            return View(new RegistrationRequestDTO());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegistrationRequestDTO registrationRequestDTO){
            registrationRequestDTO.Role = "User";
            if (ModelState.IsValid){
                var response = await _userService.RegisterAsync<ApiResponse>(registrationRequestDTO);
                if (response != null && response.IsSuccess){
                    return RedirectToAction(nameof(Login));
                }
            }
           return View(registrationRequestDTO);
        }

        public IActionResult Logout(){
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction(nameof(Login));
        }
    }
}