using MarvelApi_Api.Helpers;
using MarvelApi_Api.Models.DTOs.Jwt;
using MarvelApi_Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MarvelApi_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApiResponseHelper _apiResponseHelper;
        private readonly ILogger<TeamController> _logger;

        public UserController(IUserRepository userRepository, ApiResponseHelper apiResponseHelper, ILogger<TeamController> logger)
        {
            _apiResponseHelper = apiResponseHelper;
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var loggedInUser = await _userRepository.LoginAsync(loginRequestDTO);
                loggedInUser.Expiration = loggedInUser.Expiration.ToLocalTime();
                var response = _apiResponseHelper.GetApiResponseSuccess(loggedInUser, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                var registeredUser = await _userRepository.RegisterAsync(registrationRequestDTO);
                registeredUser.Password = "";
                var response = _apiResponseHelper.GetApiResponseSuccess(registeredUser, HttpStatusCode.OK);
                return Ok(response);
            }   
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {
            _logger.LogError("{message}", ex.Message);
            var response = _apiResponseHelper.GetApiReponseNotSuccess(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }
    }
}
