﻿using CoffeeMachine.Auth;
using CoffeeMachine.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers;

/// <summary>
/// The class that implements the authentication request
/// </summary>
[Route("api/")]
[ApiController]
public class CoffeeMachineLoginController : ControllerBase
{
    /// <summary>
    /// Get token service dependency injection
    /// </summary>
    private readonly IGetTokenService _getTokenService;

    /// <summary>
    /// Constructor of the class that implements the authentication request
    /// </summary>
    /// <param name="getTokenService"> Service for getting token </param>
    public CoffeeMachineLoginController(IGetTokenService getTokenService)
    {
        _getTokenService = getTokenService;
    }

    /// <summary>
    /// Method for obtaining a token
    /// </summary>
    /// <param name="userModel"> Data of user for login in system </param>
    /// <returns> Token </returns>
    /// <response code="200"> Success </response>
    /// <response code="401"> Invalid User </response>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserModel userModel)
    {
        var token = await _getTokenService.GetTokenAsync(userModel);

        return Ok(token);
    }
}