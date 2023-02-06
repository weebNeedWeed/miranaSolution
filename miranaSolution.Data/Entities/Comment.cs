using miranaSolution.Data.Enums;

namespace miranaSolution.Data.Entities
{
    public class Comment
    {
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CommentType CommentType { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}