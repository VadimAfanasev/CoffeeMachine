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
    }
    public string DefaultUserId { get; set; } = "1";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<CoffeeContext>(options =>
            {
                options.UseInMemoryDatabase("CoffeeMachineIntegrationTestServices" + Guid.NewGuid())
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            
        });
        builder.ConfigureTestServices(services =>
        {
            services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
        });
    }
}

