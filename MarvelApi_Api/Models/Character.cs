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
        public string Backstory { get; set; }
        public string Appearance { get; set; }
        public string Origin { get; set; }
        public List<string> Powers { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; }
        public virtual ICollection<CharacterRelationship> CharacterRelationships { get; set; } = new List<CharacterRelationship>();
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