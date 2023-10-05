using CoffeMachine.Common;
using CoffeMachine.Common.Interfaces;
using CoffeMachine.Middlewares;
using CoffeMachine.Models.Data;
using CoffeMachine.Services;
using CoffeMachine.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CoffeeContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped<ICoffeeBuyServices, CoffeeBuyServices>();
builder.Services.AddScoped<ICalculateChange, CalculateChange>();
builder.Services.AddScoped<IDecrementAvailableNotes, DecrementAvailableNotes>();
builder.Services.AddScoped<IIncrementAvailableNotes, IncrementAvailableNotes>();
builder.Services.AddScoped<IIncrementCoffeeBalances, IncrementCoffeeBalances>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseExceptionHandlerMiddleware();
//app.UseExceptionHandler(ExceptionHandlerMiddleware);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();