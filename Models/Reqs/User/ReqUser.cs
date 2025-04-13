using System.ComponentModel.DataAnnotations;

namespace Models.Reqs.User;

public record ReqUser : ReqUserSession
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(150, MinimumLength = 4)]
    public required string Name { get; init; }
}
