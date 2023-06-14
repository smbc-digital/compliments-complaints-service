using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace compliments_complaints_service.Utils.HealthChecks
{
    public class TestHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(HealthCheckResult.Healthy(null, new Dictionary<string, object> { { "Result", "All working!" } }));
        }
    }
}