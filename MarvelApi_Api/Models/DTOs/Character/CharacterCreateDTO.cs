using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Api.Models.DTOs.Character
{
    public class CharacterCreateDTO
    {
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
        public ICollection<int> EnemyIds { get; set; } = new List<int>();
        public ICollection<int> AllyIds { get; set; } = new List<int>();
    }
}