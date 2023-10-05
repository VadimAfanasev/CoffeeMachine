using CoffeMachine.Common.Interfaces;
using CoffeMachine.Models.Data;
using CoffeMachine.Services.Interfaces;
using Newtonsoft.Json;
using System.Net;

namespace CoffeMachine.Services
{
    public class CoffeeBuyServices: ICoffeeBuyServices
    {
        private CoffeeContext _db;
        private ICalculateChange _calculateChange;
        private IDecrementAvailableNotes _decrementAvailableNote;
        private IIncrementAvailableNotes _incrementAvailableNote;
        private IIncrementCoffeeBalances _incrementCoffeeBalances;

        private uint[] banknotes = new uint[] { 5000, 2000, 1000, 500 };
        public CoffeeBuyServices(CoffeeContext db, ICalculateChange calculateChange, IDecrementAvailableNotes decrementAvailableNotes, 
            IIncrementAvailableNotes incrementAvailableNotes, IIncrementCoffeeBalances incrementCoffeeBalances)
        {
            _db = db;
            _calculateChange = calculateChange;
            _decrementAvailableNote = decrementAvailableNotes;
            _incrementAvailableNote = incrementAvailableNotes;
            _incrementCoffeeBalances = incrementCoffeeBalances;
        }

        public List<uint> BuyingCoffee(string coffeeType, uint[] moneys)
        {
            if (!_db.Coffees.Any(c => c.Name == coffeeType))
            {
                throw new ArgumentException("Неверно выбран кофе", nameof(coffeeType));
            }

            if (!moneys.All(c => banknotes.Contains(c)))
            {
                //return BadRequest("Invalid banknotes type");
            }

            uint moneysUint = SumUintArray(moneys);
            uint coffeePrice = GetCoffeePrice(coffeeType);

            if (moneysUint < coffeePrice)
            {
                //return BadRequest("Insufficient amount");
            }

            var changeAmount = moneysUint - coffeePrice;

            _incrementAvailableNote.IncrementAvailableNote(moneys);

            var change = _calculateChange.Calculate(changeAmount);

            if (change == null)
            {
                //return BadRequest("Cannot provide change");
            }

            _decrementAvailableNote.DecrementAvailableNote(change);

            _incrementCoffeeBalances.IncrementCoffeeBalance(coffeeType, coffeePrice);

            return change;
        }

        public uint SumUintArray(uint[] moneys)
        {
            long sum = moneys.Sum(n => (long)n);
            return (uint)sum;
        }

        public uint GetCoffeePrice(string coffeeType)
        {
            var coffeePrice = _db.Coffees
                .Where(c => c.Name == coffeeType)
                .Select(x => x.Price).ToList();

            return coffeePrice[0];
        }
    }
}
