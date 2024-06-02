using System.ComponentModel.DataAnnotations;

namespace MarvelApi_Mvc.Models.DTOs.TeamDTOs
{
    public class TeamUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}