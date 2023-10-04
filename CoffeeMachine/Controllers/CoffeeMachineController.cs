using CoffeeMachine.Common.Order;
using CoffeeMachine.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CoffeeMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly CoffeeBuyServices _coffeeBuyServices;

        public CoffeeMachineController(CoffeeBuyServices coffeeBuyServices)
        {
            _coffeeBuyServices = coffeeBuyServices;
        }

        [HttpPost("order/{coffeeType}")]
        public ActionResult<OrderResponse> PlaceOrder(string coffeeType, [FromBody] uint[] moneys)
        {
            var change = _coffeeBuyServices.BuyingCoffee(coffeeType, moneys);

            return new OrderResponse
            {
                Change = change,
                //CoffeeBalance = servicesForRequest,
                //TotalBalance = moneyInMachine.totalBalance
            };
        }
    }
}