using MarvelApi_Mvc.Models.DTOs.UserDTOs;

namespace MarvelApi_Mvc.Services.IServices
{
    public interface IUserService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO);
    }
}