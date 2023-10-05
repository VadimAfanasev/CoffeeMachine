using CoffeMachine.Common.Order;
using Microsoft.AspNetCore.Mvc;
using CoffeMachine.Services.Interfaces;
using CoffeMachine.Dto;

namespace CoffeMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        private ICoffeeBuyServices _service;
        private IInputMoneyServices _moneyService;
        public CoffeeMachineController(ICoffeeBuyServices service, IInputMoneyServices moneyService)
        {
            _service = service;
            _moneyService = moneyService;
        }

        [HttpPost("order/{coffeeType}")]
        public ActionResult<OrderCoffeeDto> PlaceOrder(string coffeeType, [FromBody] uint[] moneys)
        {
            var change= _service.BuyingCoffee(coffeeType, moneys);

            return new OrderCoffeeDto
            {
                Change = change
            };
        }

        [HttpPut("inputing/")]
        public ActionResult<List<InputMoneyDto>> InputMoney([FromBody] List<InputMoneyDto> inputMoney)
        {
            var input = _moneyService.InputingMoney(inputMoney);
        }
    }
}