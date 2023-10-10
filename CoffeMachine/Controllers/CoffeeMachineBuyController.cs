using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers;

using CoffeeMachine.Auth;
using Microsoft.AspNetCore.Authorization;
using static CoffeeMachine.Auth.User;

[Route("api/")]
[ApiController]
public class CoffeeMachineBuyController : ControllerBase
{
    private readonly ICoffeeBuyServices _coffeeBuyService;


    public CoffeeMachineBuyController(ICoffeeBuyServices coffeeBuyService)
    {
        _coffeeBuyService = coffeeBuyService;
    }

    /// <summary>
    /// Buying coffee
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// Post /note
    /// {
    ///     title: "coffee bought"
    /// }
    /// </remarks>
    /// <param name="coffeeType">CoffeeType object</param>
    /// <param name="moneys">Moneys object</param>
    /// <returns>Returns change (guid)</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Incorrect data entered</response>
    /// <response code="404">Entity not found in the system</response>
    [HttpPost("order/{coffeeType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderCoffeeDto>> PlaceOrder(string coffeeType, [FromBody] uint[] moneys)
    {
        var change = await _coffeeBuyService.BuyingCoffeeAsync(coffeeType, moneys);

        return Ok(change);
    }
}