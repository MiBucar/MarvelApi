using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Mvc.Models.DTOs.TeamDTOs
{
    public class TeamCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        public byte[]? Image { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}