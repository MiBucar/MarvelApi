namespace MarvelApi_Api.Models.DTOs.Jwt
{
    public class LoginResponseDTO
    {
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
