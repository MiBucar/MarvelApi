using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;

namespace MarvelApi_Mvc.Models.ViewModels.Team
{
	public class DisplayTeamViewModel
	{
		public TeamDTO Team { get; set; }
		public List<CharacterDTO> Members { get; set; }
	}
}
