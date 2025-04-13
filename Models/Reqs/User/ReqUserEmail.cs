using System.ComponentModel.DataAnnotations;

namespace Models.Reqs.User;

public record ReqUserEmail : ReqBaseModel
{
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid Email")]
    [StringLength(250, MinimumLength = 4)]
    public required string Email { get; init; }
}
