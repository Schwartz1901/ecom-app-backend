using Microsoft.EntityFrameworkCore;
using Order.Domain.Repositories;
using Order.Infrastructure;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;
using Product.Domain.SeedWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("LocalDatabase");
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


