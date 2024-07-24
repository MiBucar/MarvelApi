using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Mvc.Models.DTOs.CharacterDTOs
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
        [Required]
        public List<string>? Powers { get; set; } = new List<string>();
        public int? TeamId { get; set; }
        public ICollection<int> EnemyIds { get; set; } = new List<int>();
        public ICollection<int> AllyIds { get; set; } = new List<int>();
    }
}