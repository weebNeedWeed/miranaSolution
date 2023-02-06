namespace miranaSolution.Data.Entities
{
    public class BookComment
    {
        public int CommentTypeId { get; set; }
        public CommentType CommentType { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}