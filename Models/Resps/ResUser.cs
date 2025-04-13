namespace Models.Resps;

public record ResUser
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public string? Email { get; init; }

    public DateTime CreatedAt { get; init; }
}
