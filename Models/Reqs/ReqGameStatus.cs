using Models.DTOs;

namespace Models.Reqs
{
    public record ReqGameStatus : ReqBaseModel
    {
        public required int Id { get; set; }

        public required GameStatus Status { get; set; }

        public int? Rate { get; set; }
    }
}
