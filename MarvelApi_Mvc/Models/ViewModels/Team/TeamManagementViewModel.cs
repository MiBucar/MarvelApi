using MarvelApi_Mvc.Models.DTOs.TeamDTOs;

namespace MarvelApi_Mvc.Models.ViewModels.Team
{
    public class TeamManagementViewModel
    {
        public IEnumerable<TeamDTO> Teams { get; set; }
        public string SearchQuery { get; set; }
    }
}
