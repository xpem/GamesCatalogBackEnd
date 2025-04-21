using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTOs;

[Table("User")]
public class UserDTO : DTOBase
{
    [MaxLength(150)]
    public required string Name { get; set; }

    [MaxLength(250)]
    public required string Email { get; set; }

    [MaxLength(350)]
    public required string Password { get; set; }
}