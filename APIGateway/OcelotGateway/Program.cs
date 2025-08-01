using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Load ocelot.json config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Register Ocelot with Polly support
builder.Services.AddOcelot(builder.Configuration)
                .AddPolly(); // REQUIRED when using QoSOptions

var app = builder.Build();

// Make sure to await UseOcelot()
await app.UseOcelot();

app.Run();
