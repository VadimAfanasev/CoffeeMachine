using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class CoffeeMachineController : ControllerBase
        {
            private Dictionary<string, int> coffeePrices = new Dictionary<string, int>
            {
                { "Cappuccino", 600 },
                { "Latte", 850 },
                { "Americano", 900 }
            };

            private Dictionary<int, int> availableNotes = new Dictionary<int, int>
            {
                { 500, 10 },
                { 1000, 10 },
                { 2000, 10 },
                { 5000, 10 }
            };

            private Dictionary<string, int> coffeeBalances = new Dictionary<string, int>
            {
                { "Cappuccino", 0 },
                { "Latte", 0 },
                { "Americano", 0 }
            };

            private int totalBalance = 0;

            [HttpPost("order/{coffeeType}")]
            public ActionResult<OrderResponse> PlaceOrder(string coffeeType, [FromBody] OrderRequest orderRequest)
            {
                if (!coffeePrices.ContainsKey(coffeeType))
                {
                    return BadRequest("Invalid coffee type");
                }

                var coffeePrice = coffeePrices[coffeeType];
                if (orderRequest.Amount < coffeePrice)
                {
                    return BadRequest("Insufficient amount");
                }

                var changeAmount = orderRequest.Amount - coffeePrice;
                var change = CalculateChange(changeAmount);

                if (change == null)
                {
                    return BadRequest("Cannot provide change");
                }

                DecrementAvailableNotes(change);

                coffeeBalances[coffeeType] += coffeePrice;
                totalBalance += coffeePrice;

                return new OrderResponse
                {
                    Change = change,
                    CoffeeBalance = coffeeBalances,
                    TotalBalance = totalBalance
                };
            }

            private List<int> CalculateChange(int amount)
            {
                var change = new List<int>();

                var sortedNotes = availableNotes.Keys.OrderByDescending(n => n);
                foreach (var note in sortedNotes)
                {
                    while (amount >= note && availableNotes[note] > 0)
                    {
                        change.Add(note);
                        amount -= note;
                        availableNotes[note]--;
                    }
                }

                return amount == 0 ? change : null;
            }

            private void DecrementAvailableNotes(List<int> change)
            {
                foreach (var note in change)
                {
                    availableNotes[note]--;
                }
            }
        }

        public class OrderRequest
        {
            public int Amount { get; set; }
        }

        public class OrderResponse
        {
            public List<int> Change { get; set; }
            public Dictionary<string, int> CoffeeBalance { get; set; }
            public int TotalBalance { get; set; }
        }
    
}