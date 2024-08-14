using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.UserDTOs;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public async Task<ActionResult> Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.LoginAsync<ApiResponse>(loginRequestDTO);
                if (response.Result != null && response.IsSuccess)
                {
                    var dataObject = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(dataObject.Token);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                    var usernameClaim = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
                    var roleClaim = jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value;

                    if (usernameClaim != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, usernameClaim));
                    }

                    if (roleClaim != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim));
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetString("JWToken", dataObject.Token);
                    return RedirectToAction("Index", "Home");
                }
                else if (response != null && !response.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, response.ErrorMessages.FirstOrDefault());
                }
            }
            return View(loginRequestDTO);
        }

        public async Task<ActionResult> Register()
        {
            return View(new RegistrationRequestDTO());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.RegisterAsync<ApiResponse>(registrationRequestDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Login));
                }
                else if (!response.IsSuccess)
                    ModelState.AddModelError(string.Empty, response.ErrorMessages.FirstOrDefault());
            }
            return View(registrationRequestDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}