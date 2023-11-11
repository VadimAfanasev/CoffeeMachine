using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers;

/// <summary>
/// The class implements requests for purchasing coffee from a coffee machine
/// </summary>
[Route("api/")]
[ApiController]
public class CoffeeMachineBuyController : ControllerBase
{
    /// <summary>
    /// Coffee buy service dependency injection
    /// </summary>
    private readonly ICoffeeBuyServices _coffeeBuyService;

    /// <summary>
    /// Constructor of a class that implements request for buying coffee
    /// </summary>
    /// <param name="coffeeBuyService"> Service for buying coffee </param>
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
    /// title: "coffee bought"
    /// }
    /// </remarks>
    /// <param name="coffeeType"> CoffeeType object </param>
    /// <param name="moneys"> Moneys object </param>
    /// <returns> Returns change (guid) </returns>
    /// <response code="200"> Success </response>
    /// <response code="400"> Incorrect data entered </response>
    /// <response code="404"> Entity not found in the system </response>
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