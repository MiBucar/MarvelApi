namespace MarvelApi_Mvc.Models.DTOs.UserDTOs
{
    public class RegistrationRequestDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}