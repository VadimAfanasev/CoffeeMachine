using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;

namespace CoffeeMachine.Controllers
{
    using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("coffeebalance")]
        public async Task<IActionResult> GetCoffeeBalanceAsync()
        {
            var coffeeBalance = await _coffeeMachineStatusService.GetBalanceCoffeeAsync();
            return Ok(coffeeBalance);
        }

        [HttpGet("moneyinmachine")]
        public async Task<IActionResult> GetMoneyInMachineAsync()
        {
            var moneyInMachine = await _coffeeMachineStatusService.GetBalanceMoneyAsync();
            return Ok(moneyInMachine);
        }

        [HttpPut("inputing/")]
        public async Task<ActionResult<List<InputMoneyDto>>> InputMoneyAsync([FromBody] List<InputMoneyDto> inputMoney)
        {
            await _inputMoneyService.InputingAsync(inputMoney);
            return Ok();
        }

        [HttpPost("order/{coffeeType}")]
        public async Task<ActionResult<OrderCoffeeDto>> PlaceOrder(string coffeeType, [FromBody] uint[] moneys)
        {
            var change = await _coffeeBuyService.BuyingCoffeeAsync(coffeeType, moneys);

            return Ok(change);
        }
    }
}