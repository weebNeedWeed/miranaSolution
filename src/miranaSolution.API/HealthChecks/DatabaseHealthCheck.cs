using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using miranaSolution.Data.Main;

namespace miranaSolution.API.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly MiranaDbContext _dbContext;

    public DatabaseHealthCheck(MiranaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var conn = _dbContext.Database.GetDbConnection();
        await conn.OpenAsync();

        // Select 1 to check whether the database can return value or not
        var command = conn.CreateCommand();
        command.CommandText = "SELECT 1";

        var reader = await command.ExecuteReaderAsync();
        var isHealthy = reader.HasRows; // Save the result before close the connection;
        await conn.CloseAsync();

        if (isHealthy) return HealthCheckResult.Healthy();

        return HealthCheckResult.Unhealthy();
    }
}