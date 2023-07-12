namespace miranaSolution.Data.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid? UserId { get; set; }
    public AppUser? AppUser { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public List<CommentReaction> CommentReactions { get; set; } = new();
}