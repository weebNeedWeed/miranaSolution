namespace miranaSolution.Data.Entities;

public class Bookmark
{
    public int ChapterId { get; set; }
    public Chapter? Chapter { get; set; }

    public Guid UserId { get; set; }
    public AppUser? AppUser { get; set; }

    public DateTime CreatedAt { get; set; }
}