using Microsoft.AspNetCore.Identity;

namespace miranaSolution.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Book> Books { get; set; }

        public List<Rating> Ratings { get; set; }
    }
}