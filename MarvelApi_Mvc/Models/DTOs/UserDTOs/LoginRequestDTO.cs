using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Mvc.Models.DTOs.UserDTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}