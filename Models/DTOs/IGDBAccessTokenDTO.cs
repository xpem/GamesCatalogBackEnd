using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTOs
{
    [Table("IGDBAccessToken")]
    public class IGDBAccessTokenDTO
    {
        public int Id { get; set; }

        public required DateTime UpdatedAt { get; set; }

        [MaxLength(250)]
        public required string AccessToken { get; set; }

        public required int ExpiresIn { get; set; }

        [MaxLength(100)]
        public required string TokenType { get; set; }
    }
}
