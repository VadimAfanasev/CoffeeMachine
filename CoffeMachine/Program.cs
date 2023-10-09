using System.Reflection;

using CoffeeMachine.Common;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Middlewares;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CoffeeContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped<ICoffeeBuyServices, CoffeeBuyServices>();
builder.Services.AddScoped<ICalculateChange, CalculateChange>();
builder.Services.AddScoped<IDecrementAvailableNotes, DecrementAvailableNotes>();
builder.Services.AddScoped<IIncrementAvailableNotes, IncrementAvailableNotes>();
builder.Services.AddScoped<IIncrementCoffeeBalances, IncrementCoffeeBalances>();
builder.Services.AddScoped<IInputMoneyServices, InputMoneyServices>();
builder.Services.AddScoped<IIncrementMoneyInMachine, IncrementMoneyInMachine>();
builder.Services.AddScoped<ICoffeeMachineStatusServices, CoffeeMachineStatusServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();