using CoffeeMachine.Auth;
using CoffeeMachine.Common;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Middlewares;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

builder.Services.AddOptions<Jwt>()
    .Bind(builder.Configuration.GetSection(nameof(Jwt)))
    .ValidateDataAnnotations();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Pathnostics", Version = "v1" });
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
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
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IGetTokenService, GetTokenService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();