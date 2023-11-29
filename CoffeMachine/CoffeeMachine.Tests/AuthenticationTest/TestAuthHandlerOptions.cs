using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CoffeeMachine.Tests.AuthenticationTest;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
    public virtual ClaimsIdentity Identity { get; set; } = new ClaimsIdentity(
        new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            //new Claim(ClaimTypes.Email, "test@test.test"),
            new Claim(ClaimTypes.Name, "Test Testington"),
            new Claim(ClaimTypes.Role, "technician")

        }, "test");
}