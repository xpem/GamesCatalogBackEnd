using Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Models.Reqs
{
    public record ReqGame: ReqBaseModel
    {
        public required int IGDBId { get; set; }

        [MaxLength(350)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public string? ReleaseDate { get; set; }

        [MaxLength(350)]
        public string? Platforms { get; set; }

        public string? Summary { get; set; }

        public string? CoverUrl { get; set; }

        public required GameStatus Status { get; set; }

        public int? Rate { get; set; }
    }
}