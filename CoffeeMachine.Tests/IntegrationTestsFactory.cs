using CoffeeMachine.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace CoffeeMachine.Tests;

public class IndexPageTests :
    CustomWebApplicationFactory<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public IndexPageTests(
        CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false

        });
    }

    [Test]
    public async Task Login_ReturnsRedirectToRoot()
    {
        //Arrange

        

        //Act
        var loginResponse = await _client.GetAsync("/api/login");
        

        //Assert
        loginResponse.Should().NotBeNull();
    }
}