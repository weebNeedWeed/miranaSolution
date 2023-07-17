namespace miranaSolution.Data.Entities;

public class CommentReaction
{
    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }

    public int CommentId { get; set; }
    public Comment? Comment { get; set; }
}