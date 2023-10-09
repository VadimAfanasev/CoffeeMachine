using static CoffeeMachine.Common.EnumBanknotes;

namespace CoffeeMachine.Services
{
    using CoffeeMachine.Common.Interfaces;
    using CoffeeMachine.Dto;
    using CoffeeMachine.Models.Data;
    using CoffeeMachine.Services.Interfaces;

    public class CoffeeBuyServices : ICoffeeBuyServices
    {
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

        // Метод с помощью которого осуществляется покупка кофе
        public async Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys)
        {
            if (!_db.Coffees.Any(c => c.Name == coffeeType))
                throw new InvalidDataException("Invalid coffee type");

            if (!moneys.All(c => Enum.IsDefined(typeof(Banknotes), c)))
                throw new InvalidDataException("Invalid banknotes type");

            // Получаем сумму внесенных средств и стоимость кофе для вычисления сдачи
            var moneysUint = SumUintArray(moneys);
            var coffeePrice = GetCoffeePrice(coffeeType);

            if (moneysUint < coffeePrice)
                throw new ArgumentException("The amount deposited is less than required");

            var changeAmount = moneysUint - coffeePrice;

            // Метод в котором асинхронно вычисляется сдача из доступных купюр по оптимальному алгоритму.
            var change = await _calculateChange.CalculateAsync(changeAmount);

            if (change == null)
                throw new ArgumentException("Cannot provide change");

            // Добавляем внесенные пользователем деньги в таблицу
            await _incrementAvailableNote.IncrementAvailableNoteAsync(moneys);

            // Выдаем сдачу пользователю, пишем изменения в таблицу
            await _decrementAvailableNote.DecrementAvailableNoteAsync(change);

            //Добавляем купленный пользователем кофе в таблицу
            await _incrementCoffeeBalances.IncrementCoffeeBalanceAsync(coffeeType, coffeePrice);

            var changeDto = ChangeToDto(change);

            return changeDto;
        }

        // Вычисляем сумму внесенных средств для вычисления сдачи
        public uint SumUintArray(uint[] moneys)
        {
            var sum = moneys.Sum(n => n);
            return (uint)sum;
        }

        // Передаем сдачу пользователя в ДТО
        private static OrderCoffeeDto ChangeToDto(List<uint> change)
        {
            var changeDto = new OrderCoffeeDto
            {
                Change = change
            };

            return changeDto;
        }

        // Вычисляем стоимость кофе для вычисления сдачи
        private uint GetCoffeePrice(string coffeeType)
        {
            var coffeePrice = _db.Coffees
                .Where(c => c.Name == coffeeType)
                .Select(x => x.Price).ToList();

            return coffeePrice[0];
        }
    }
}