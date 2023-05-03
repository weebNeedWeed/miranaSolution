using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.Data.Main
{
    public class MiranaDbContextFactory : IDesignTimeDbContextFactory<MiranaDbContext>
    {
        public MiranaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MiranaDbContext>();
            optionsBuilder.UseSqlServer(this.GetAppConnectionString());

            return new MiranaDbContext(optionsBuilder.Options);
        }

        private string GetAppConnectionString()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();

            return root.GetConnectionString(SystemConstants.DatabaseSettings.DEFAULT_CONNECTION_STRING);
        }
    }
}