using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Mvc.Models.DTOs.CharacterDTOs
{
    public class CharacterDTO
    {
        [Key]
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
        public int? TeamId { get; set; }
        public string Team { get; set; }
        public List<string> Powers { get; set; }
        public List<int> AllyIds { get; set; } = new List<int>();
        public List<int> EnemyIds { get; set; } = new List<int>();
    }
}