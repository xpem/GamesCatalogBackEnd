namespace Models.DTOs;

public class DTOBase
{
    public int Id { get; set; }

    public required DateTime CreatedAt { get; set; }
}
