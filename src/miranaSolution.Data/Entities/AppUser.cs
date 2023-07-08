using Microsoft.AspNetCore.Identity;

namespace miranaSolution.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Book> Books { get; set; }

        public List<Rating> Ratings { get; set; }

        public List<Comment> Comments { get; set; }
        
        public List<Bookmark> Bookmarks { get; set; }
        
        public string? Avatar { get; set; }
    }
}