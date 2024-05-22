using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Api.Models.DTOs.Hero
{
    public class HeroDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsVillain { get; set; }
        public byte[]? Image { get; set; }
    }
}