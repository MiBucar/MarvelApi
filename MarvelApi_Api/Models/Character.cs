using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarvelApi_Api.Models;

namespace MarvelApi_Api.Models
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVillain { get; set; }
        public byte[]? Image { get; set; }
        public string ImageType { get; set; }
        public string Backstory { get; set; }
        public string Appearance { get; set; }
        public int FirstAppearanceYear { get; set; }
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
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; }
        public virtual ICollection<CharacterRelationship> CharacterRelationships { get; set; } = new List<CharacterRelationship>();
        public int? TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }

    public class CharacterRelationship
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int RelatedCharacterId { get; set; }
        public Character RelatedCharacter { get; set; }

        public bool IsEnemy { get; set; }
    }
}