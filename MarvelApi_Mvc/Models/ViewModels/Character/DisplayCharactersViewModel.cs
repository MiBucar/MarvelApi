using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarvelApi_Mvc.Models.ViewModels.Character
{
    public class DisplayCharactersViewModel
    {
        public List<CharacterDTO> CharacterDTOs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }
    }
}