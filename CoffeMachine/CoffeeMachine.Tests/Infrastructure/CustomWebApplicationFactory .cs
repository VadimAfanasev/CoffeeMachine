using CoffeeMachine.Tests.AuthenticationTest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;

namespace CoffeeMachine.Tests.Infrastructure;

internal class CustomWebApplicationFactory : WebApplicationFactory<CoffeeMachineBuyController>
{
    private readonly ExternalServicesMock _externalServicesMock;

    public CustomWebApplicationFactory(ExternalServicesMock externalServicesMock)
    {
        _externalServicesMock = externalServicesMock;
        _dbId = Guid.NewGuid();
    }
    public string DefaultUserId { get; set; } = "1";
    private Guid _dbId;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");
#pragma warning disable CS0618
        builder.UseSerilog((_, _) => { });
#pragma warning restore CS0618
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<CoffeeContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            var dbDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(CoffeeContext));
            if (dbDescriptor != null)
                services.Remove(dbDescriptor);

            services.AddDbContext<CoffeeContext>(options =>
            {
                options.UseInMemoryDatabase("CoffeeMachineIntegrationTestServices" + _dbId)
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<CoffeeContext>())
            {
                    appContext.Database.EnsureCreated();
                    TestDbBaseContext.GetTestInitAppContext(appContext);
            }
        });
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = TestAuthHandler.AUTHENTICATION_SCHEME;
                options.DefaultChallengeScheme = TestAuthHandler.AUTHENTICATION_SCHEME;
            }
            )
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AUTHENTICATION_SCHEME,
                    options => { });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("administrator", b =>
                {
                    b.RequireRole("administrator");
                });
                opts.AddPolicy("technician", b =>
                {
                    b.RequireRole("technician");
                });

            });
        });
        base.ConfigureWebHost(builder);
        Log.Logger = new LoggerConfiguration()
           .CreateLogger();
    }
}

