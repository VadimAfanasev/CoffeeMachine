using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers;

/// <summary>
/// The class that implements the requests for obtaining the status of the coffee machine
/// </summary>
[Route("api/")]
[ApiController]
public class CoffeeMachineAdministratorController : ControllerBase
{
    /// <summary>
    /// Coffee status service dependency injection
    /// </summary>
    private readonly ICoffeeMachineStatusServices _coffeeMachineStatusService;

    /// <summary>
    /// Input money service dependency injection
    /// </summary>
    private readonly IInputMoneyServices _inputMoneyService;

    /// <summary>
    /// Constructor of the class that obtaining the status of the coffee machine
    /// </summary>
    /// <param name="inputMoneyService"> Service for inputting money in machine </param>
    /// <param name="coffeeMachineStatusService"> Service for getting machine status </param>
    public CoffeeMachineAdministratorController(IInputMoneyServices inputMoneyService,
        ICoffeeMachineStatusServices coffeeMachineStatusService)
    {
        _inputMoneyService = inputMoneyService;
        _coffeeMachineStatusService = coffeeMachineStatusService;
    }

    /// <summary>
    /// Get the amount of purchased coffee from the coffee machine
    /// </summary>
    /// <response code="200"> Success </response>
    /// <response code="404"> Entity not found in the system </response> 
    [Authorize(policy: "technician")]
    [HttpGet("coffeebalance")]
    [ProducesResponseType(typeof(BalanceCoffeeDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCoffeeBalanceAsync()
    {
        var coffeeBalance = await _coffeeMachineStatusService.GetBalanceCoffeeAsync();
        return Ok(coffeeBalance);
    }

    /// <summary>
    /// Get the amount of money from the coffee machine
    /// </summary>
    /// <response code="200"> Success </response>
    /// <response code="404"> Entity not found in the system </response>
    [Authorize(policy: "technician")]
    //[DisableCors]
    [HttpGet("moneyinmachine")]
    [ProducesResponseType(typeof(MoneyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMoneyInMachineAsync()
    {
        var moneyInMachine = await _coffeeMachineStatusService.GetBalanceMoneyAsync();
        return Ok(moneyInMachine);
    }

    /// <summary>
    /// Deposit money into the coffee machine
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// Put /note
    /// {
    /// title: "money deposited"
    /// }
    /// </remarks>
    /// <param name="inputMoney"> InputMoneyDto object </param>
    /// <returns> Returns NoContent </returns>
    /// <response code="200"> Success </response>
    /// <response code="400"> Invalid banknotes type </response>
    [Authorize(policy: "administrator")]
    [HttpPut("inputing")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> InputMoneyAsync([FromBody] List<MoneyDto> inputMoney)
    {
        var answer = await _inputMoneyService.InputingAsync(inputMoney);
        return Ok(answer);
    }
}