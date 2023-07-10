namespace miranaSolution.DTOs.Catalog.Comments;

public class CommentDto
{
    public int Id { get; set; }
    
    public string? Content { get; set; }
    
    public int? ParentId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public Guid? UserId { get; set; }

    public int BookId { get; set; }
    
    public int ReactionCount { get; set; }
}