namespace miranaSolution.Data.Entities;

public class CurrentlyReading
{
    public int ChapterIndex { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public Guid UserId { get; set; }
    public AppUser? AppUser { get; set; }

    public DateTime CreatedAt { get; set; }
}