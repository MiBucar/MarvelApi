using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Api.Models.DTOs.Team
{
    public class TeamCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}