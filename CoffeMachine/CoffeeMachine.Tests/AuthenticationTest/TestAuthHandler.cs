using Microsoft.AspNetCore.Authentication;

namespace CoffeeMachine.Tests.AuthenticationTest;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}