using miranaSolution.Data.Enums;

namespace miranaSolution.Data.Entities
{
    public class Reaction
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }

        public ReactionType ReactionType { get; set; }
    }
}