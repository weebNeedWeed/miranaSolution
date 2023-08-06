namespace miranaSolution.Data.Entities;

public class BookRating
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public string Content { get; set; } = string.Empty;

    public int Star { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}