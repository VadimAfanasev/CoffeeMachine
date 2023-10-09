using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly ICoffeeBuyServices _coffeeBuyService;
        private readonly ICoffeeMachineStatusServices _coffeeMachineStatusService;
        private readonly IInputMoneyServices _inputMoneyService;

        public CoffeeMachineController(ICoffeeBuyServices coffeeBuyService, IInputMoneyServices inputMoneyService,
            ICoffeeMachineStatusServices coffeeMachineStatusService)
        {
            _coffeeBuyService = coffeeBuyService;
            _inputMoneyService = inputMoneyService;
            _coffeeMachineStatusService = coffeeMachineStatusService;
        }

        /// <summary>
        /// Get the amount of purchased coffee from the coffee machine
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Entity not found in the system</response>
        [HttpGet("coffeebalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCoffeeBalanceAsync()
        {
            var coffeeBalance = await _coffeeMachineStatusService.GetBalanceCoffeeAsync();
            return Ok(coffeeBalance);
        }

        /// <summary>
        /// Get the amount of money from the coffee machine
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Entity not found in the system</response>
        [HttpGet("moneyinmachine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        ///     title: "money deposited"
        /// }
        /// </remarks>
        /// <param name="inputMoney">InputMoneyDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid banknotes type</response>
        [HttpPut("inputing/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<InputMoneyDto>>> InputMoneyAsync([FromBody] List<InputMoneyDto> inputMoney)
        {
            await _inputMoneyService.InputingAsync(inputMoney);
            return Ok();
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
}