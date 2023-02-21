namespace miranaSolution.Data.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}