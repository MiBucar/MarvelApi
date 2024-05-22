using System.Net;

namespace MarvelApi_Api.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
    }
}