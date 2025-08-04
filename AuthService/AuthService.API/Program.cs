using AuthService.Domain.Repositories;
using AuthService.Domain.SeedWork;
using AuthService.Infrastructure;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7004/api");
});

builder.Services.AddHttpClient("CartService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7003/api");
});
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LocalDatabase");
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();


