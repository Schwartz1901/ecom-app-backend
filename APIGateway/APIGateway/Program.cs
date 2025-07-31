using Polly;
using Polly.Extensions.Http;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

IAsyncPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() 
    .OrResult(response => response.StatusCode == HttpStatusCode.NotFound)
    .WaitAndRetryAsync(3,
    retryAttempt => TimeSpan.FromSeconds(4),
    onRetry: (outcome, delay, retryAttempt, context) =>
    {
        Console.WriteLine($"[Polly Retry #{retryAttempt}] Delay: {delay.TotalSeconds}s, Reason: " +
            $"{outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
    });
IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );

builder.Services.AddHttpClient("productCluster")
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);
builder.Services.AddHttpClient("userCluster")
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
    

var app = builder.Build();

app.MapReverseProxy();

app.Run();

