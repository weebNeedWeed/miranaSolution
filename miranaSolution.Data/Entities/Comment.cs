namespace miranaSolution.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string CommentTypeId { get; set; }
        public CommentType CommentType { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}