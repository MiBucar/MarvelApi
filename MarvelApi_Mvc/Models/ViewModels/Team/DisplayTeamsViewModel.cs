using MarvelApi_Mvc.Models.DTOs.TeamDTOs;

namespace MarvelApi_Mvc.Models.ViewModels.Team
{
    public class DisplayTeamsViewModel
    {
        public List<TeamDTO> TeamDTOs = new List<TeamDTO>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
