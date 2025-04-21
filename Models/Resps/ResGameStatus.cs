using Models.DTOs;

namespace Models.Resps
{
    public record ResGameStatus
    {
        public int Id { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required GameStatus Status { get; set; }

        public int? Rate { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Inactive { get; set; }

        public ResGame? Game { get; set; }
    }
}