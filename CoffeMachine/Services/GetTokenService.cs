using CoffeeMachine.Auth;
using CoffeeMachine.Services.Interfaces;
using static CoffeeMachine.Auth.User;

namespace CoffeeMachine.Services;

public class GetTokenService : IGetTokenService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public GetTokenService(ITokenService tokenService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<string> GetTokenAsync(UserModel userModel)
    {
        var userDto = _userRepository.GetUser(userModel);

        if (userDto == null)
            throw new UnauthorizedAccessException("Invalid User");

        var token = _tokenService.BuildToken(_httpContextAccessor.HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:Key"],
            _httpContextAccessor.HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:Issuer"], userDto);

        return token;
    }


}