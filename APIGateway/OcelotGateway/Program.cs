using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Load ocelot.json config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Register Ocelot with Polly support
builder.Services.AddOcelot(builder.Configuration)
                .AddPolly();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5025); // HTTP
    serverOptions.ListenAnyIP(7123, listenOptions =>
    {
        listenOptions.UseHttps(); // Enable HTTPS
    });
});
var app = builder.Build();

// Make sure to await UseOcelot()
app.UseHttpsRedirection();
await app.UseOcelot();


app.Run();
