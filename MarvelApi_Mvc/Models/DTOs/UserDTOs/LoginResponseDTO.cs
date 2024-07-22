namespace MarvelApi_Mvc.Models.DTOs.UserDTOs
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}