using CoffeeMachine.Tests.AuthenticationTest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoffeeMachine.Tests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<CoffeeMachineBuyController>
{
    private readonly ExternalServicesMock _externalServicesMock;

    public CustomWebApplicationFactory(ExternalServicesMock externalServicesMock)
    {
        _externalServicesMock = externalServicesMock;
        _dbID = Guid.NewGuid();
    }
    public string DefaultUserId { get; set; } = "1";
    private Guid _dbID;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");
        base.ConfigureWebHost(builder);
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
                options.UseInMemoryDatabase("CoffeeMachineIntegrationTestServices" + _dbID)
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<CoffeeContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                    TestDbBaseContext.GetTestInitAppContext(appContext);
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }

        });
        builder.ConfigureTestServices(services =>
        {
            services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme,
                    options => { });
        });
    }
}

