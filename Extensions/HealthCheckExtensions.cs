using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Catalog.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void ConfigureHealthCheck(this WebApplication app)
        {
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready"),
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            checks = report.Entries.Select(x => new
                            {
                                name = x.Key,
                                status = x.Value.Status.ToString(),
                                exception = x.Value.Exception != null
                                    ? x.Value.Exception.Message
                                    : "none",
                                duration = x.Value.Duration.ToString()
                            })
                        }
                    );

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });

            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = (_) => false
            });
        }
    }
}