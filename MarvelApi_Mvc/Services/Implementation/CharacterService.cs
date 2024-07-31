using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Services.IServices;
using static MarvelApi_Mvc.Utilities.SD;

namespace MarvelApi_Mvc.Services.Implementation
{
    public class CharacterService : BaseService, ICharacterService
    {
        private string _marvelUrl;
        public CharacterService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory) {
            _marvelUrl = configuration.GetValue<string>("ServiceUrls:MarvelApi");
        }
    
        public async Task<T> CreateAsync<T>(CharacterCreateDTO characterCreateDTO)
        {
            ApiRequest apiRequest = new ApiRequest{
                ApiData = characterCreateDTO,
                ApiType = ApiType.POST,
                ApiUrl = _marvelUrl + "/api/Character"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            ApiRequest apiRequest = new ApiRequest{
                ApiData = id,
                ApiType = ApiType.DELETE,
                ApiUrl = _marvelUrl + $"/api/Character/{id}"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAllAsync<T>()
        {
            ApiRequest apiRequest = new ApiRequest{
                ApiType = ApiType.GET,
                ApiUrl = _marvelUrl + "/api/Character/"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAsync<T>(int id)
        {
            ApiRequest apiRequest = new ApiRequest{
                ApiData = id,
                ApiType = ApiType.GET,
                ApiUrl = _marvelUrl + $"/api/Character/{id}"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> SearchAsync<T>(string query)
        {
            ApiRequest apiRequest = new ApiRequest{
                ApiData = query,
                ApiType = ApiType.GET,
                ApiUrl = _marvelUrl + $"/api/Character/{query}"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateAsync<T>(CharacterUpdateDTO characterUpdateDTO)
        {
            ApiRequest apiRequest = new ApiRequest{
                ApiData = characterUpdateDTO,
                ApiType = ApiType.PUT,
                ApiUrl = _marvelUrl + $"/api/Character/{characterUpdateDTO.Id}"
            };
            return await SendAsync<T>(apiRequest);
        }
    }
}