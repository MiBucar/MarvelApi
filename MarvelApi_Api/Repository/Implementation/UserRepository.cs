using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Jwt;
using Microsoft.EntityFrameworkCore;

namespace MarvelApi_Api.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<bool> IsUniqueUserAsync(string username)
        {
            return !await _db.Users.AnyAsync(x => x.UserName == username);
        }

        public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<User> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
