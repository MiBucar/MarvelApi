using MarvelApi_Mvc.Models.DTOs.CharacterDTOs;

namespace MarvelApi_Mvc.Models.ViewModels
{
    public class CharacterCreateViewModel
    {
        public CharacterCreateDTO CharacterCreateDTO { get; set; }

        public CharacterCreateViewModel()
        {
            CharacterCreateDTO = new CharacterCreateDTO();
        }

        public string NewPower { get; set; }
        public string PowerToDelete { get; set; }

        public void AddNewPower()
        {
            if (!string.IsNullOrEmpty(NewPower) && !CharacterCreateDTO.Powers.Contains(NewPower))
            {
                CharacterCreateDTO.Powers.Add(NewPower);
                NewPower = string.Empty;
            }
        }

        public void DeletePower()
        {
            if (CharacterCreateDTO.Powers.Contains(PowerToDelete))
            {
                CharacterCreateDTO.Powers.Remove(PowerToDelete);
                PowerToDelete = string.Empty;
            }
        }
    }
}