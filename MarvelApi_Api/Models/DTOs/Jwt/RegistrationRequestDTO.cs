﻿namespace MarvelApi_Api.Models.DTOs.Jwt
{
    public class RegistrationRequestDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
