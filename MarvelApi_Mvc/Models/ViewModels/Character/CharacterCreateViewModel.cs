using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarvelApi_Mvc.Models.ViewModels.Character
{
    public class CharacterCreateViewModel
    {
        public CharacterCreateDTO CharacterCreateDTO { get; set; }
        public List<SelectListItem> AvailableCharacters { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableTeams { get; set; } = new List<SelectListItem>();
    }
}