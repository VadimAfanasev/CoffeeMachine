using CoffeMachine.Common.Interfaces;
using CoffeMachine.Models.Data;

namespace CoffeMachine.Common
{
    public class IncrementCoffeeBalances: IIncrementCoffeeBalances
    {
        private readonly CoffeeContext _db;
        public IncrementCoffeeBalances(CoffeeContext db)
        {
            _db = db;
        }

        public void IncrementCoffeeBalance(string coffeeType, uint coffeePrice)
        {
            var coffeeName = _db.CoffeeBalances.FirstOrDefault(c => c.Name == coffeeType);

            if (coffeeName != null)
            {
                coffeeName.Balance = coffeeName.Balance + coffeePrice;
            }
            _db.SaveChanges();
        }
    }
}
