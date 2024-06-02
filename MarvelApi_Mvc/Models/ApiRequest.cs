using static MarvelApi_Mvc.Utilities.SD;

namespace MarvelApi_Mvc.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; }
        public string ApiUrl { get; set; }
        public object ApiData { get; set; }
    }
}
