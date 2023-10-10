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
    public async Task<ActionResult> Login([FromBody] UserModel userModel)
    {
        var token = await _getTokenService.GetTokenAsync(userModel);

        return Ok(token);
    }
}