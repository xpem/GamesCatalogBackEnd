using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTOs
{
    [Table("Game")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IGDBId), IsUnique = true)]
    public class GameDTO : DTOBase
    {
        public int? IGDBId { get; set; }

        [MaxLength(350)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public string? ReleaseDate { get; set; }

        [MaxLength(350)]
        public string? Platforms { get; set; }

        public string? Summary { get; set; }

        public string? CoverUrl { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
