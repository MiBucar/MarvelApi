using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;

namespace MarvelApi_Mvc.Models.ViewModels.Character
{
    public class CharactersManagementViewModel
    {
        public IEnumerable<CharacterDTO> Characters { get; set; }
        public string SearchQuery { get; set; }
    }
}
