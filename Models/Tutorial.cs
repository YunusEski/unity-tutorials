namespace UnityTutorialSite.Models
{
    public class Tutorial
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string LastUpdated { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public List<string> Steps { get; set; } = new List<string>();
    }
}
