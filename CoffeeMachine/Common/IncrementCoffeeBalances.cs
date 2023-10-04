using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common
{
    public class IncrementCoffeeBalances
    {
        private readonly ApplicationContext _db;
        public IncrementCoffeeBalances(ApplicationContext db)
        {
            _db = db;
        }
        public IncrementCoffeeBalances()
        {
        }

        public void IncrementCoffeeBalance(string coffeeType, uint coffeePrice)
        {
            var coffeeName = _db.CoffeeBalances.FirstOrDefault(c => c.CoffeeName == coffeeType);

            if (coffeeName != null)
            {
                coffeeName.Balance = coffeeName.Balance + coffeePrice;
            }
            _db.SaveChanges();
        }
    }
}
