namespace miranaSolution.Data.Entities;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string ThumbnailImage { get; set; } = string.Empty;
    public bool IsRecommended { get; set; }
    public string Slug { get; set; } = string.Empty;
    public bool IsDone { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public Guid? UserId { get; set; }
    public AppUser? AppUser { get; set; }

    public List<Chapter> Chapters { get; set; } = new();

    public List<BookGenre> BookGenres { get; set; } = new();

    public List<Rating> Ratings { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
    
    public List<BookUpvote> BookUpvotes { get; set; } = new();
    
    public List<Bookmark> Bookmarks { get; set; } = new();
}