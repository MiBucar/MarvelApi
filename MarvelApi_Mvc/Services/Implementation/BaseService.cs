using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using MarvelApi_Mvc.Models;
using MarvelApi_Mvc.Services.IServices;
using Newtonsoft.Json;
using static MarvelApi_Mvc.Utilities.SD;

namespace MarvelApi_Mvc.Services.Implementation
{
    public class BaseService : IBaseService
    {
        public ApiResponse ApiResponse { get; set; }
        private readonly IHttpClientFactory _clientFactory;


        public BaseService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> SendAsync<T>(ApiRequest request)
        {
            try
            {
                HttpClient httpClient = _clientFactory.CreateClient("MarvelApi");
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.RequestUri = new Uri(request.ApiUrl);

                switch (request.ApiType)
                {
                    case ApiType.GET:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                    case ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        requestMessage.Method = HttpMethod.Delete;
                        break;
                }

                if (request.ApiData != null)
                {
                    var json = JsonConvert.SerializeObject(request.ApiData);
                    requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
                var content = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                ApiResponse apiResponse = new ApiResponse
                {
                    ErrorMessages = new List<string>() { Convert.ToString(ex.Message) },
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false
                };

                var json = JsonConvert.SerializeObject(apiResponse);
                var APIResponse = JsonConvert.DeserializeObject<T>(json);
                return APIResponse;
            }
        }
    }
}