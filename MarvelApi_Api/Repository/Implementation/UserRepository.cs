using MarvelApi_Api.Data;
using MarvelApi_Api.Models;
using MarvelApi_Api.Models.DTOs.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
                                                                && x.Password == loginRequestDTO.Password);

            if (user == null)
                return new LoginResponseDTO
                {
                    User = null,
                    Token = "",
                    Expiration = DateTime.UtcNow,
                };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim(ClaimTypes.Name, user.Id.ToString()),
                   new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponseDTO
            {
                User = user,
                Expiration = token.ValidTo,
                Token = tokenString
            };
        }

        public async Task<User> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            var userExists = await _db.Users.AnyAsync(x => x.UserName.ToLower() == registrationRequestDTO.UserName.ToLower());
            if (userExists)
                throw new Exception("User already exists");

            var newUser = new User()
            {
                Name = registrationRequestDTO.UserName,
                UserName = registrationRequestDTO.UserName,
                Password = registrationRequestDTO.Password,
                Role = registrationRequestDTO.Role,
            };

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();

            return newUser;
        }
    }
}
