using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Api.Models.DTOs.Team
{
    public class TeamUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile ImageForm { get; set; }
        public byte[] Image { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}