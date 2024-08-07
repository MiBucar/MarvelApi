namespace MarvelApi_Api.Models.DTOs.Team
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public List<string> Members { get; set; } = new List<string>();
    }
}