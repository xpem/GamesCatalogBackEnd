using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.DTOs
{
    [Table("GameStatus")]
    public class GameStatusDTO : DTOBase
    {
        public required int GameId { get; set; }

        public int UserId { get; set; }    

        public required GameStatus Status { get; set; }

        public int? Rate { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool Inactive { get; set; } = false;

        [JsonIgnore]
        public GameDTO? Game { get; set; }
    }

    public enum GameStatus
    {
        Want,
        Playing,
        Played
    }
}

