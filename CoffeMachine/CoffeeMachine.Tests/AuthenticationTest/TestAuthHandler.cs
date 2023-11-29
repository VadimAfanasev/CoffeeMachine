using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CoffeeMachine.Tests.AuthenticationTest;

public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
    const string USER_ID = "UserId";

    public const string AUTHENTICATION_SCHEME = "Test";
    private readonly string _defaultUserId;

    public TestAuthHandler(
        IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _defaultUserId = options.CurrentValue.DefaultUserId;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claimName = new Claim(ClaimTypes.Role, "administrator");
        var claimAdmin = new Claim(ClaimTypes.Role, "technician");
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user"), claimName, claimAdmin };

        if (Context.Request.Headers.TryGetValue(USER_ID, out var userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
        }

        var identity = new ClaimsIdentity(claims, AUTHENTICATION_SCHEME);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AUTHENTICATION_SCHEME);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}