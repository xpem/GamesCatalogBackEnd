using System.ComponentModel.DataAnnotations;

namespace Models.Reqs.IGDB
{
    public record ReqIGDBGamesSearch
    {
        [Required(ErrorMessage = "Search is required")]
        [StringLength(250, MinimumLength = 4)]
        public required string Search { get; init; }

        public required int StartIndex { get; init; }
    }
}
