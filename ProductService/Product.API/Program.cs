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
var blobStorageString = builder.Configuration.GetConnectionString("AzureBlobStorage");
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddBlobServiceClient(blobStorageString);
    
});
builder.Services.AddSingleton(sp =>
{
    var svc = sp.GetRequiredService<BlobServiceClient>();
    var cfg = sp.GetRequiredService<IConfiguration>();
    var containerName = cfg["Storage:Container"] ?? "thao";
    var container = svc.GetBlobContainerClient(containerName);

    // Create once at startup (remove if container is provisioned by IaC)
    container.CreateIfNotExists(); // or CreateIfNotExists(PublicAccessType.Blob) if you want public reads

    return container;
});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
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
