namespace MarvelApi_Mvc.Models.DTOs.TeamDTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Members { get; set; } = new List<string>();
    }
}