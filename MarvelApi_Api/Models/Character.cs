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
        public ICollection<Character> Enemies { get; set; } = new List<Character>();
        public ICollection<Character> Allies { get; set; } = new List<Character>();
    }
}