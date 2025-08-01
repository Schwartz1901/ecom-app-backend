using Microsoft.EntityFrameworkCore;
using Order.API.Interfaces;
using Order.API.Services;
using Order.Domain.Repositories;
using Order.Infrastructure;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;
using Order.Domain.SeedWork;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LocalDatabase");
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IOrderService, OrderService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();
app.Run();


