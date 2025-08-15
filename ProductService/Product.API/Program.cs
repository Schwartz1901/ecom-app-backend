using Microsoft.EntityFrameworkCore;
using Product.API.Interfaces;
using Product.API.Services;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("LocalDatabase");

builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));

var blobCfg = builder.Configuration.GetSection("AzureBlob");
builder.Services.AddSingleton(new BlobServiceClient(blobCfg["ConnectionString"]!));
builder.Services.AddScoped<IBlobService, BlobService>(sp =>
{
    var svc = sp.GetRequiredService<BlobServiceClient>();
    var container = blobCfg["Container"]!;
    return new BlobService(svc, container);
});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // Angular dev server
            .AllowAnyMethod()
            .AllowAnyHeader();
        //.AllowCredentials();  cookies/auth
    });
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("Angular");
app.MapControllers();

app.Run();
