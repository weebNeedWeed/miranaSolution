namespace miranaSolution.Data.Entities;

public class BookUpvote
{
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}