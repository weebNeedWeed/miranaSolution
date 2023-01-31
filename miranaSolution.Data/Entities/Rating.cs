namespace miranaSolution.Data.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public int ReactionCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}