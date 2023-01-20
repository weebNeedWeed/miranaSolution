using Microsoft.EntityFrameworkCore;

namespace miranaSolution.Data.Main
{
    public class MiranaDbContext : DbContext
    {
        public MiranaDbContext(DbContextOptions<MiranaDbContext> options) : base(options)
        {
        }
    }
}