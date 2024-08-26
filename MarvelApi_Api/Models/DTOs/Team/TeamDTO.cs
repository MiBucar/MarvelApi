namespace MarvelApi_Api.Models.DTOs.Team
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<int> MembersIds { get; set; } = new List<int>();
    }
}