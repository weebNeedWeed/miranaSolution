namespace miranaSolution.Data.Entities;

public class Rating
{
    public string? Content { get; set; }
    public int? ParentId { get; set; }
    public float Stars { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid UserId { get; set; }
    public AppUser? AppUser { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }
}