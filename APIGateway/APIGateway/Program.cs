using Polly;
using Polly.Extensions.Http;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );

builder.Services.AddHttpClient("productCluster")
    .AddPolicyHandler(GetRetryPolicy());
//.AddPolicyHandler(circuitBreakerPolicy);
builder.Services.AddHttpClient("userCluster")
    .AddPolicyHandler(GetRetryPolicy());
//.AddPolicyHandler(circuitBreakerPolicy);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(response => response.StatusCode == HttpStatusCode.NotFound)
    .WaitAndRetryAsync(3,
    retryAttempt => TimeSpan.FromSeconds(4),
    onRetry: (outcome, delay, retryAttempt, context) =>
    {
        Console.WriteLine($"[Polly Retry #{retryAttempt}] Delay: {delay.TotalSeconds}s, Reason: " +
            $"{outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
    });
}