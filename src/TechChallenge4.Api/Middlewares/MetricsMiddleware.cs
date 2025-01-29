using Prometheus;
using System.Diagnostics;

namespace TechChallenge4.Api.Middlewares
{
    public class MetricsMiddleware
    {
        private static readonly Counter RequestCounter = Metrics.CreateCounter("http_requests_total", "Total number of HTTP requests", new[] { "method", "endpoint", "status" });

        private static readonly Histogram LatencyHistogram = Metrics.CreateHistogram("http_request_duration_seconds", "Histogram of HTTP request duration in seconds", new[] { "method", "endpoint" });

        private readonly RequestDelegate _next;

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode.ToString();
            var method = context.Request.Method;
            var endpoint = context.GetEndpoint()?.DisplayName ?? "";

            // Atualiza o contador e o histograma com as métricas coletadas
            RequestCounter.WithLabels(method, endpoint, statusCode).Inc();
            LatencyHistogram.WithLabels(method, endpoint).Observe(stopwatch.Elapsed.TotalSeconds);
        }
    }
}
