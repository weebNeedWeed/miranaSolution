using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Main
{
    public class MiranaDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public MiranaDbContext(DbContextOptions<MiranaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}