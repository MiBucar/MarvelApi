using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Models.DTOs.UserDTOs;
using MarvelApi_Mvc.Services.IServices;
using static MarvelApi_Mvc.Utilities.SD;

namespace MarvelApi_Mvc.Services.Implementation
{
    public class UserService : BaseService, IUserService
    {

        private string _marvelUrl;

        public UserService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _marvelUrl = configuration.GetValue<string>("ServiceUrls:MarvelApi");
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            var request = new ApiRequest{
                ApiType = ApiType.POST,
                ApiData = loginRequestDTO,
                ApiUrl = _marvelUrl + "/api/User/login"
            };
            return await SendAsync<T>(request);
        }

        public async Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO)
        {
            var request = new ApiRequest{
                ApiType = ApiType.POST,
                ApiData = registrationRequestDTO,
                ApiUrl = _marvelUrl + "/api/User/register"
            };
            return await SendAsync<T>(request);
        }
    }
}