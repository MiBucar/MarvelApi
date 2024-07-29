using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;
using MarvelApi_Mvc.Models.DTOs.TeamDTOs;

namespace MarvelApi_Mvc.Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<CharacterDTO> CharacterDTOs = new List<CharacterDTO>();
        public List<TeamDTO> TeamDTOs = new List<TeamDTO>();
    }
}