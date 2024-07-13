using MarvelApi_Mvc.Models.DTOs.TeamDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarvelApi_Mvc.Models.ViewModels.Team
{
    public class TeamUpdateViewModel
    {
        public TeamUpdateDTO TeamUpdateDTO { get; set; }
        public List<SelectListItem> AvailableCharacters { get; set; } = new List<SelectListItem>();
    }
}
