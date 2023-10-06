namespace CoffeMachine.Controllers
{
    using CoffeMachine.Dto;
    using CoffeMachine.Services.Interfaces;

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
        public IActionResult GetCoffeeBalance()
        {
            var coffeeBalance = _coffeeMachineStatusService.GetBalanceCoffee();
            return Ok(coffeeBalance);
        }

        [HttpGet("moneyinmachine")]
        public IActionResult GetMoneyInMachine()
        {
            var moneyInMachine = _coffeeMachineStatusService.GetBalanceMoney();
            return Ok(moneyInMachine);
        }

        [HttpPut("inputing/")]
        public ActionResult<List<InputMoneyDto>> InputMoney([FromBody] List<InputMoneyDto> inputMoney)
        {
            _inputMoneyService.Inputing(inputMoney);
            return Ok();
        }

        [HttpPost("order/{coffeeType}")]
        public ActionResult<OrderCoffeeDto> PlaceOrder(string coffeeType, [FromBody] uint[] moneys)
        {
            var change = _coffeeBuyService.BuyingCoffee(coffeeType, moneys);

            return Ok(change);
        }
    }
}