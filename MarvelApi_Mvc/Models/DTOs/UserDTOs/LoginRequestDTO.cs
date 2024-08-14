using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Mvc.Models.DTOs.UserDTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}