using CoffeeMachine.Common;
using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Services
{
    public class CoffeeBuyServices
    {
        private ApplicationContext _db;
        private CalculateChange calculateChange = new CalculateChange();
        private DecrementAvailableNotes decrementAvailableNote = new DecrementAvailableNotes();
        private IncrementAvailableNotes incrementAvailableNote = new IncrementAvailableNotes();
        private IncrementCoffeeBalances incrementCoffeeBalances = new IncrementCoffeeBalances();

        private uint[] banknotes = new uint[] { 5000, 2000, 1000, 500 };
        public CoffeeBuyServices(ApplicationContext db)
        {
            _db = db;
        }

        public List<uint> BuyingCoffee(string coffeeType, uint[] moneys)
        {
            if (!_db.Coffees.Any(c => c.Name == coffeeType))
            {
                //return BadRequest("Invalid coffee type");
            }

            if (!moneys.All(c => banknotes.Contains(c)))
            {
                //return BadRequest("Invalid banknotes type");
            }

            var coffeePrice = _db.Coffees
                .Where(c => c.Name == coffeeType)
                .Select(x => x.Price);

            if (SumUintArray(moneys) < Convert.ToUInt32(coffeePrice))
            {
                //return BadRequest("Insufficient amount");
            }

            var changeAmount = SumUintArray(moneys) - Convert.ToUInt32(coffeePrice);

            incrementAvailableNote.IncrementAvailableNote(moneys);

            var change = calculateChange.Calculate(changeAmount);

            if (change == null)
            {
                //return BadRequest("Cannot provide change");
            }

            decrementAvailableNote.DecrementAvailableNote(change);

            incrementCoffeeBalances.IncrementCoffeeBalance(coffeeType, Convert.ToUInt32(coffeePrice));

            return change;
        }

        public static uint SumUintArray(uint[] moneys)
        {
            long sum = moneys.Sum(n => (long)n);
            return (uint)sum;
        }
    }
}
