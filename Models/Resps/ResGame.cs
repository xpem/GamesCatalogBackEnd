namespace Models.Resps
{
    public record ResGame
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? IGDBId { get; set; }

        public string? Name { get; set; }

        public string? ReleaseDate { get; set; }

        public string? Platforms { get; set; }

        public string? Summary { get; set; }

        public string? CoverUrl { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}