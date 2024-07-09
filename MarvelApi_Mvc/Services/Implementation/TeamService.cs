using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.Extensions.Configuration;
using static MarvelApi_Mvc.Utilities.SD;

namespace MarvelApi_Mvc.Services.Implementation
{
    public class TeamService : BaseService, ITeamService
    {
        private string _marvelUrl;
        public TeamService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _marvelUrl = configuration.GetValue<string>("ServiceUrls:MarvelApi");
        }

        public async Task<T> CreateAsync<T>(TeamCreateDTO teamCreateDTO)
        {
            ApiRequest apiRequest = new ApiRequest
            {
                ApiData = teamCreateDTO,
                ApiType = ApiType.POST,
                ApiUrl = _marvelUrl + "/api/Team"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            ApiRequest apiRequest = new ApiRequest
            {
                ApiData = id,
                ApiType = ApiType.DELETE,
                ApiUrl = _marvelUrl + $"/api/Team/{id}"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAllAsync<T>()
        {
            ApiRequest apiRequest = new ApiRequest
            {
                ApiType = ApiType.GET,
                ApiUrl = _marvelUrl + "/api/Team/"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAsync<T>(int id)
        {
            ApiRequest apiRequest = new ApiRequest
            {
                ApiData = id,
                ApiType = ApiType.GET,
                ApiUrl = _marvelUrl + $"/api/Team/{id}"
            };
            return await SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateAsync<T>(TeamUpdateDTO teamUpdateDTO)
        {
            ApiRequest apiRequest = new ApiRequest
            {
                ApiData = teamUpdateDTO,
                ApiType = ApiType.PUT,
                ApiUrl = _marvelUrl + $"/api/Team/{teamUpdateDTO.Id}"
            };
            return await SendAsync<T>(apiRequest);
        }
    }
}
