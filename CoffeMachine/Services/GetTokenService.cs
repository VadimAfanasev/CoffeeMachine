using CoffeeMachine.Auth;
using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Settings;

using Microsoft.Extensions.Options;


namespace CoffeeMachine.Services;

/// <summary>
/// The class that implements obtaining a token
/// </summary>
public class GetTokenService : IGetTokenService
{
    /// <summary>
    /// Implementing Jwt configurations
    /// </summary>
    private readonly IOptions<Jwt> _jwtOptions;

    /// <summary>
    /// Injecting token creation methods
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Implementing a list of users available for authentication
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Constructor of the class in which we receive the token
    /// </summary>
    /// <param name="tokenService"> Token Service </param>
    /// <param name="userRepository"> User repository </param>
    /// <param name="jwtOptions"> JWT options </param>
    public GetTokenService(ITokenService tokenService, IUserRepository userRepository,
        IOptions<Jwt> jwtOptions)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _jwtOptions = jwtOptions;
    }

    /// <inheritdoc />
    public async Task<string> GetTokenAsync(UserModel userModel)
    {
        var userDto = await _userRepository.GetUserAsync(userModel);

        if (userDto == null)
            throw new UnauthorizedAccessException("Invalid User");

        var token = _tokenService.BuildToken(_jwtOptions.Value.Key, _jwtOptions.Value.Issuer, userDto);

        return token;
    }
}