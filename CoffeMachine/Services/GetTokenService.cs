using CoffeeMachine.Auth;
using CoffeeMachine.Services.Interfaces;

using static CoffeeMachine.Auth.User;

namespace CoffeeMachine.Services;

/// <summary>
/// The class that implements obtaining a token
/// </summary>
public class GetTokenService : IGetTokenService
{
    /// <summary>
    /// Injecting token creation methods
    /// </summary>
    private readonly ITokenService _tokenService;
    /// <summary>
    /// Implementing a list of users available for authentication
    /// </summary>
    private readonly IUserRepository _userRepository;
    /// <summary>
    /// Implementing context Http
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor of the class in which we receive the token
    /// </summary>
    public GetTokenService(ITokenService tokenService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public async Task<string> GetTokenAsync(UserModel userModel)
    {
        var userDto = await _userRepository.GetUserAsync(userModel);

        if (userDto == null)
            throw new UnauthorizedAccessException("Invalid User");

        var token = _tokenService.BuildToken(_httpContextAccessor.HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:Key"],
            _httpContextAccessor.HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:Issuer"], userDto);

        return token;
    }
}