using Microsoft.Extensions.Diagnostics.HealthChecks;

using Npgsql;

namespace CoffeeMachine.HealthChecks;

/// <summary>
/// Database status monitoring functionality
/// </summary>
public class DatabaseHealthChecks : IHealthCheck
{
    /// <summary>
    /// Connection string
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Constructor of a class that describes the functionality for monitoring the state of the database
    /// </summary>
    /// <param name="configuration"></param>
    public DatabaseHealthChecks(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// A method that implements monitoring the state of the database
    /// </summary>
    /// <param name="context"> Health check context </param>
    /// <param name="cancellationToken">  </param>
    /// <returns> Health check result </returns>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";

            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                context.Registration.FailureStatus.ToString(),
                exception: ex);
        }
    }
}