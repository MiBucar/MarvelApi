using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarvelApi_Mvc.Models.ViewModels.Team
{
    public class TeamCreateViewModel
    {
        public TeamCreateDTO TeamCreateDTO { get; set; }
        public List<SelectListItem> AvailableCharacters { get; set; } = new List<SelectListItem>();
    }
}
