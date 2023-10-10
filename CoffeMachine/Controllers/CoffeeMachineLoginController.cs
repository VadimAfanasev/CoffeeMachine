using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers;

using CoffeeMachine.Auth;
using Microsoft.AspNetCore.Authorization;
using static CoffeeMachine.Auth.User;

[Route("api/")]
[ApiController]
public class CoffeeMachineLoginController : ControllerBase
{
    private readonly IGetTokenService _getTokenService;

    public CoffeeMachineLoginController(IGetTokenService getTokenService)
    {
        _getTokenService = getTokenService;
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] UserModel userModel)
    {
        var token = _getTokenService.GetTokenAsync(userModel);

        return Ok(token.Result);
    }

    //public ActionResult Login([FromBody] UserModel userModel)
    //{
    //    UserModel userModelNew = new UserModel()
    //    {
    //        UserName = userModel.UserName,
    //        Password = userModel.Password
    //    };

    //    var userDto = _userRepository.GetUser(userModelNew);

    //    if (userDto == null)
    //        throw new UnauthorizedAccessException("Invalid User");

    //    var token = _tokenService.BuildToken(HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:Key"],
    //        HttpContext.RequestServices.GetService<IConfiguration>()["Jwt:Issuer"],
    //        userDto);

    //    return Ok(token);
    //}
}