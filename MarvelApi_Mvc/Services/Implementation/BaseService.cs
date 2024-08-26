using System.Collections;
using System.Net;
using System.Net.Http.Headers;
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
        private string _token;

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

                if (!string.IsNullOrEmpty(_token)){
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                }  

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

                if (request.ApiData != null && (requestMessage.Method != HttpMethod.Get && requestMessage.Method != HttpMethod.Delete))
                {
                    var content = new MultipartFormDataContent();

                    var properties = request.ApiData.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        var propValue = prop.GetValue(request.ApiData);
                        if (propValue is IFormFile file)
                        {
                            var streamContent = new StreamContent(file.OpenReadStream());
                            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = prop.Name,
                                FileName = file.FileName
                            };
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                            content.Add(streamContent);
                        }
                        else if (propValue is IEnumerable<int> collection2)
                        {
                            int index = 0;
                            foreach (var item in collection2)
                            {
                                content.Add(new StringContent(item.ToString()), $"{prop.Name}[{index}]");
                                index++;
                            }
                        }
                        else if (propValue is IEnumerable<object> collection && !(propValue is string))
                        {
                            int index = 0;
                            foreach (var item in collection)
                            {
                                content.Add(new StringContent(item?.ToString() ?? string.Empty), $"{prop.Name}[{index}]");
                                index++;
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(propValue?.ToString() ?? string.Empty), prop.Name);
                        }
                    }

                    requestMessage.Content = content;
                }

                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
                var contentString = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(contentString);
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