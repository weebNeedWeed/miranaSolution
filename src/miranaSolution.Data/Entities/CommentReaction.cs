namespace miranaSolution.Data.Entities;

public class CommentReaction
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }

    public int CommentId { get; set; }
    public Comment? Comment { get; set; }
}