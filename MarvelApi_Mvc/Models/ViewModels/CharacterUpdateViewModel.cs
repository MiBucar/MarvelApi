using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarvelApi_Mvc.Models.ViewModels
{
    public class CharacterUpdateViewModel
    {
        public CharacterUpdateDTO CharacterUpdateDTO { get; set; }
        public List<SelectListItem> AvailableCharacters { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableTeams { get; set; } = new List<SelectListItem>();
    }
}
