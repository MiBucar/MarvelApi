using MarvelApi_Mvc.Models;

namespace MarvelApi_Mvc.Services.IServices
{
    public interface IBaseService
    {
        ApiResponse ApiResponse { get; set; }
        Task<T> SendAsync<T>(ApiRequest request);
    }
}