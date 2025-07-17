using DocumentAPI.Interfaces;
using DocumentAPI.Models;
using DocumentAPI.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
// DI
builder.Services.AddDbContext<DocumentDbContext>(opt => opt.UseInMemoryDatabase("DocumentDb"));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IExternalAPIService, ExternalAPIService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev port
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandler?.Error;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            // Mapping from Exception to HttpSatusCode
            ArgumentException => StatusCodes.Status400BadRequest,

            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            status = context.Response.StatusCode,
            message = exception?.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseSwagger(); 
app.UseSwaggerUI();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
