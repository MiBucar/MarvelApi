using System.ComponentModel.DataAnnotations;
using MarvelApi_Api.Models.DTOs.Team;

namespace MarvelApi_Api.Models.DTOs.CharacterDTOS
{
    public class CharacterCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public bool IsVillain { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        public string Backstory { get; set; }
        [Required]
        public string Appearance { get; set; }
        [Required]
        public int FirstAppearanceYear { get; set; }
        [Required]
        public string Origin { get; set; }
		public string Powers { get; set; }
		public int Durability { get; set; }
		public int Energy { get; set; }
		public int FightingSkills { get; set; }
		public int Strength { get; set; }
		public int Intelligence { get; set; }
		public int Speed { get; set; }
		public string Height { get; set; }
		public string Eyes { get; set; }
		public string Weight { get; set; }
		public string Hair { get; set; }
		public int? TeamId { get; set; }
        public ICollection<int>? EnemyIds { get; set; } = new List<int>();
        public ICollection<int>? AllyIds { get; set; } = new List<int>();
    }
}