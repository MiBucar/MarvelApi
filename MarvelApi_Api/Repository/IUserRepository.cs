using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Jwt;

namespace MarvelApi_Api.Repository
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUserAsync(string username);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<User> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
    }
}
