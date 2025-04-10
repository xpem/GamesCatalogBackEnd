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


//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Models.DTOs;

//[Table("Game")]
//[Microsoft.EntityFrameworkCore.Index(nameof(IGDBId), IsUnique = true)]
//public class IGDBGameDTO : DTOBase
//{
//    [MaxLength(100)]
//    public required int IGDBId { get; set; }

//    [MaxLength(300)]
//    public required string Name { get; set; }

//    public DateTime? ReleaseDate { get; set; }

//    [MaxLength(1000)]
//    public string? Platforms { get; set; }

//    public string? Summary { get; set; }

//    [MaxLength(2500)]
//    public string? CoverUrl { get; set; }
//}