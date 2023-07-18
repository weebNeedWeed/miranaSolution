using Microsoft.AspNetCore.Identity;

namespace miranaSolution.Data.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string Avatar { get; set; } = string.Empty;

    public List<Rating> Ratings { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();

    public List<Bookmark> Bookmarks { get; set; } = new();

    public List<CommentReaction> CommentReactions { get; set; } = new();
    
    public List<BookUpvote> BookUpvotes { get; set; } = new();
}