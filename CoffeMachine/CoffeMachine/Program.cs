using CoffeeMachine.Common;
using CoffeeMachine.Common.Constants;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.HealthChecks;
using CoffeeMachine.Middlewares;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;

using HealthChecks.UI.Client;

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.OpenSearch;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

builder.Logging.ClearProviders();
Log.Logger = ConfigureLogging();
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    options =>
    {
        //const string oauth2 = "OAuth2";
        //const string bearer = "Bearer";
        var securityScheme = new OpenApiSecurityScheme
        {
            Description = "Swagger",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            Scheme = SwaggerConstants.OAUTH2,
            BearerFormat = "JWT",
            Flows = new OpenApiOAuthFlows
            {
                Password = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(builder.Configuration["Keycloak:Auth"]!),
                    TokenUrl = new Uri(builder.Configuration["Keycloak:TokenExchange"]!)
                }
            },
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = SwaggerConstants.OAUTH2
            }
        };
        var securitySchemeBearer = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = SwaggerConstants.BEARER
            }
        };

        options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeMachine", Version = "v1" });
        options.AddSecurityDefinition(SwaggerConstants.OAUTH2, securityScheme);
        options.AddSecurityDefinition(SwaggerConstants.BEARER, securitySchemeBearer);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                securityScheme, new string[] { }
            }
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                securitySchemeBearer, new string[] { }
            }
        });
    });

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CoffeeContext>(options => options.UseNpgsql(connection));

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthChecks>("Database", HealthStatus.Unhealthy);

builder.Services.AddScoped<ICoffeeBuyServices, CoffeeBuyServices>();
builder.Services.AddScoped<IChangeCalculation, ChangeCalculationService>();
builder.Services.AddScoped<IInputMoneyServices, InputMoneyServices>();
builder.Services.AddScoped<IIncrementMoneyInMachine, IncrementMoneyInMachineService>();
builder.Services.AddScoped<ICoffeeMachineStatusServices, CoffeeMachineStatusServices>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddHttpContextAccessor();

var authenticationOptions = new KeycloakAuthenticationOptions
{
    AuthServerUrl = "http://localhost:8080/",
    Realm = "CoffeeMachine",
    Resource = "CoffeeMachine",
    VerifyTokenAudience = false,
    SslRequired = "none",
};
var authorizationOptions = new KeycloakProtectionClientOptions
{
    AuthServerUrl = "http://localhost:8080/",
    Realm = "CoffeeMachine",
    Resource = "CoffeeMachine",
    VerifyTokenAudience = false,
    SslRequired = "none",

};

builder.Services.AddKeycloakAuthentication(authenticationOptions);
builder.Services.AddAuthorization(o =>
{
    o.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    o.AddPolicy(PolicyConstants.TECHNICIAN_CONST, b =>
    {
        b.RequireRealmRoles(PolicyConstants.TECHNICIAN_CONST);
    });
    o.AddPolicy(PolicyConstants.ADMINISTRATOR_CONST, b =>
    {
        b.RequireRealmRoles(PolicyConstants.ADMINISTRATOR_CONST);
    });
}).AddKeycloakAuthorization(authorizationOptions);

builder.Services.AddLazyCache();

//const string myOrigins = "_myOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsConstants.MY_ORIGINS,
        policy =>
        {
            policy.WithOrigins("http://localhost:5026")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseCors(CorsConstants.MY_ORIGINS);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Coffee machine");
        options.OAuthClientId(builder.Configuration["Keycloak:ClientId"]);
        options.OAuthClientSecret(builder.Configuration["Keycloak:ClientSecret"]);
        options.OAuthRealm("CoffeeMachine");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();

Logger ConfigureLogging()
{
    const string environment = "Development";
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile(
            $"appsettings.{environment}.json",
            false)
        .Build();

    SelfLog.Enable(msg => Console.WriteLine(msg));

    var openSearchOptions = new OpenSearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        MinimumLogEventLevel = LogEventLevel.Debug,
        IndexFormat =
            $"coffee-machine-{DateTime.UtcNow:yyyy-MM-dd}",
        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
        EmitEventFailure = EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.ThrowException,
        TypeName = "_doc",
        InlineFields = false
    };
    var logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.OpenSearch(openSearchOptions)
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    return logger;
}