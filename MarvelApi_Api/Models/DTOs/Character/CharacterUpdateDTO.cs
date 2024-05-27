using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Api.Models.DTOs.Character
{
    public class CharacterUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsVillain { get; set; }
        public byte[]? Image { get; set; }
        [Required]
        public string Backstory { get; set; }
        [Required]
        public string Appearance { get; set; }
        [Required]
        public string Origin { get; set; }
        public List<string> Powers { get; set; }
    }
}