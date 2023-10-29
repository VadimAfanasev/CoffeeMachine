using CoffeeMachine.Auth;
using CoffeeMachine.Common;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.HealthChecks;
using CoffeeMachine.Middlewares;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.OpenSearch;
using System;
using System.Globalization;
using System.Net;
using System.Reflection;

using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;

using OpenSearch.Net;
using Elastic.CommonSchema;

using ElasticsearchSinkOptions = Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions;
using EmitEventFailureHandling = Serilog.Sinks.Elasticsearch.EmitEventFailureHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
Serilog.Log.Logger = ConfigureLogging();
builder.Host.UseSerilog();
//2
//builder.Logging.ClearProviders();
//SelfLog.Enable(msg => Console.WriteLine(msg));
//ServicePointManager.ServerCertificateValidationCallback = (source, certificate, chain, sslpolicyerrors) => true;

//4
//SelfLog.Enable(msg => Console.WriteLine(msg));
//ServicePointManager.ServerCertificateValidationCallback =
//    (source, certificate, chain, sslPolicyErrors) => true;


builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    options =>
    {
        const string oauth2 = "OAuth2";
        const string bearer = "Bearer";
        var securityScheme = new OpenApiSecurityScheme
        {
            Description = "Swagger",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            Scheme = oauth2,
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
                Id = oauth2
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
                Id = bearer
            }
        };

        options.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeMachine", Version = "v1" });
        options.AddSecurityDefinition(oauth2, securityScheme);
        options.AddSecurityDefinition(bearer, securitySchemeBearer);
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

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IGetTokenService, GetTokenService>();

var authenticationOptions = new KeycloakAuthenticationOptions
{
    AuthServerUrl = "http://localhost:8080/",
    Realm = "CoffeeMachine",
    Resource = "CoffeeMachine",
    VerifyTokenAudience = false,
    SslRequired = "none",
};
builder.Services.AddKeycloakAuthentication(authenticationOptions);

//3
//var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile(
//        $"appsettings.{environment}.json",
//        optional: true)
//    .Build();
//Log.Logger = new LoggerConfiguration()
//    .Enrich.FromLogContext()
//    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
//    {
//        AutoRegisterTemplate = true,
//        IndexFormat = $"{"coffeemachine"}-{DateTime.UtcNow:yyyy-MM-dd}"
//    })
//    .Enrich.WithProperty("Environment", environment)
//    .ReadFrom.Configuration(configuration)
//    .CreateLogger();

//4
//var jsonFormatter = new CompactJsonFormatter();
//var loggerConfig = new LoggerConfiguration()
//    .Enrich.FromLogContext()
//    .WriteTo.Map("Name", "**error**", (name, writeTo) =>
//    {
//        var currentDay = DateTime.Today.Day;
//        writeTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("<opensearch endpoint>"))
//        {
//            CustomFormatter = jsonFormatter,
//            TypeName = "_doc",
//            IndexFormat = $"my-index-{currentDay}",
//            MinimumLogEventLevel = LogEventLevel.Information,
//            EmitEventFailure = EmitEventFailureHandling.RaiseCallback |
//                               EmitEventFailureHandling.ThrowException,
//            FailureCallback = e =>
//                Console.WriteLine(
//                    "An error occured in Serilog ElasticSearch sink: " +
//                    $"{e.Exception.Message} | {e.Exception.InnerException?.Message}")
//        });
//    });
//Log.Logger = loggerConfig.CreateLogger();

//5
//var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile(
//        $"appsettings.{environment}.json",
//        optional: true)
//    .Build();
//Serilog.Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .Enrich.FromLogContext()
//    .WriteTo.Elasticsearch(new[] { new Uri(configuration["ElasticConfiguration:Uri"]) }, opts =>
//    {
//        opts.DataStream = new DataStreamName("logs", "console-example", "demo");
//        opts.BootstrapMethod = BootstrapMethod.Failure;
//        opts.ConfigureChannel = channelOpts =>
//        {

//        };
//    })
//    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"])));
//ConfigureLogging(builder);
//builder.Host.UseSerilog();

//2
//SelfLog.Enable(msg => Console.WriteLine(msg));

//var logger = new LoggerConfiguration()
//.WriteTo.Console()
//    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
//    {
//        AutoRegisterTemplate = true,
//        MinimumLogEventLevel = LogEventLevel.Information,
//        FailureCallback = e => Console.WriteLine("unable to submit event " + e.MessageTemplate),
//        EmitEventFailure = EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.ThrowException,
//        TypeName = "_doc",
//        InlineFields = false

//    })
//    .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

var app = builder.Build();

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

//2
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

//2
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();

//4
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

// Serilog.Log.CloseAndFlush();
//1
//void ConfigureLogging(WebApplicationBuilder builder)
//{
//    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//    var configuration = new ConfigurationBuilder()
//        .AddJsonFile("appsettings.json", false, true)
//        .AddJsonFile(
//            $"appsettings.{environment}.json",
//            false)
//        .Build();

//    SelfLog.Enable(msg => Console.WriteLine(msg));

//    //ServicePointManager.ServerCertificateValidationCallback =
//    //    (source, certificate, chain, sslPolicyErrors) => true;

//    // builder.Logging.ClearProviders();

//    var logger = new LoggerConfiguration()
//        .Enrich.FromLogContext()
//        .Enrich.WithExceptionDetails()
//        .WriteTo.Debug()
//        .WriteTo.Console()
//        .WriteTo.OpenSearch("http://localhost:9200")
//        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
//        .Enrich.WithProperty("Environment", environment)
//        .ReadFrom.Configuration(configuration)
//        .CreateLogger();
//    builder.Logging.AddSerilog(logger);
//}

Serilog.Core.Logger ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile(
            $"appsettings.{environment}.json",
            false)
        .Build();

    SelfLog.Enable(msg => Console.WriteLine(msg));

    //ServicePointManager.ServerCertificateValidationCallback =
    //    (source, certificate, chain, sslPolicyErrors) => true;

    // builder.Logging.ClearProviders();
    var openSearchOptions = new OpenSearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        MinimumLogEventLevel = LogEventLevel.Debug,
        IndexFormat =
            $"coffee-machine-{DateTime.UtcNow:yyyy-MM-dd}",
        //NumberOfReplicas = 1,
        //NumberOfShards = 2,
        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
        EmitEventFailure = Serilog.Sinks.OpenSearch.EmitEventFailureHandling.RaiseCallback | Serilog.Sinks.OpenSearch.EmitEventFailureHandling.ThrowException,
        TypeName = "_doc",
        InlineFields = false

    };
    var logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.OpenSearch(openSearchOptions)
//        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    return logger;
}
//1
ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        MinimumLogEventLevel = LogEventLevel.Debug,
        IndexFormat =
            $"coffee-machine-{DateTime.UtcNow:yyyy-MM-dd}",
        //NumberOfReplicas = 1,
        //NumberOfShards = 2,
        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
        EmitEventFailure = EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.ThrowException,
        TypeName = "_doc",
        InlineFields = false
    };
}