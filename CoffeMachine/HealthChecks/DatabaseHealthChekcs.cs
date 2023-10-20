using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Npgsql;

namespace CoffeeMachine.HealthChecks
{
    public class DatabaseHealthChekcs : IHealthCheck
    {
        private readonly string _connectionString;

        public DatabaseHealthChekcs(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var Connection = new NpgsqlConnection(_connectionString);

                await Connection.OpenAsync(cancellationToken);

                using var command = Connection.CreateCommand();
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
}
