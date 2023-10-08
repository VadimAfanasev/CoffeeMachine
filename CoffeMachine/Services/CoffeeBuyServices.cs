using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

namespace CoffeeMachine.Services
{
    public class CoffeeBuyServices : ICoffeeBuyServices
    {
        private readonly uint[] _banknotes = { 5000, 2000, 1000, 500 };
        private readonly ICalculateChange _calculateChange;
        private readonly CoffeeContext _db;
        private readonly IDecrementAvailableNotes _decrementAvailableNote;
        private readonly IIncrementAvailableNotes _incrementAvailableNote;
        private readonly IIncrementCoffeeBalances _incrementCoffeeBalances;

        public CoffeeBuyServices(CoffeeContext db, ICalculateChange calculateChange,
            IDecrementAvailableNotes decrementAvailableNotes,
            IIncrementAvailableNotes incrementAvailableNotes, IIncrementCoffeeBalances incrementCoffeeBalances)
        {
            _db = db;
            _calculateChange = calculateChange;
            _decrementAvailableNote = decrementAvailableNotes;
            _incrementAvailableNote = incrementAvailableNotes;
            _incrementCoffeeBalances = incrementCoffeeBalances;
        }

        public async Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys)
        {
            if (!_db.Coffees.Any(c => c.Name == coffeeType))
                throw new InvalidDataException("Invalid coffee type");

            if (!moneys.All(c => _banknotes.Contains(c)))
            {
                throw new InvalidDataException("Invalid banknotes type");
            }

            var moneysUint = SumUintArray(moneys);
            var coffeePrice = GetCoffeePrice(coffeeType);

            if (moneysUint < coffeePrice)
            {
                throw new ArgumentException("The amount deposited is less than required");
            }

            var changeAmount = moneysUint - coffeePrice;

            var change = await _calculateChange.CalculateAsync(changeAmount);

            if (change == null)
            {
                throw new ArgumentException("Cannot provide change");
            }

            await _incrementAvailableNote.IncrementAvailableNoteAsync(moneys);

            await _decrementAvailableNote.DecrementAvailableNoteAsync(change);

            await _incrementCoffeeBalances.IncrementCoffeeBalanceAsync(coffeeType, coffeePrice);

            var changeDto = ChangeToDto(change);

            return changeDto;
        }

        public uint SumUintArray(uint[] moneys)
        {
            var sum = moneys.Sum(n => n);
            return (uint)sum;
        }

        private static OrderCoffeeDto ChangeToDto(List<uint> change)
        {
            var changeDto = new OrderCoffeeDto
            {
                Change = change
            };

            return changeDto;
        }

        private uint GetCoffeePrice(string coffeeType)
        {
            var coffeePrice = _db.Coffees
                .Where(c => c.Name == coffeeType)
                .Select(x => x.Price).ToList();

            return coffeePrice[0];
        }
    }
}