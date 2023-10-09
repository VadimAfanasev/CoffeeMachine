using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common
{
    public class IncrementCoffeeBalances : IIncrementCoffeeBalances
    {
        private readonly CoffeeContext _db;

        public IncrementCoffeeBalances(CoffeeContext db)
        {
            _db = db;
        }

        //Добавляем купленный пользователем кофе в таблицу
        public async Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice)
        {
            var coffeeName = await _db.Coffees.FirstOrDefaultAsync(c => c.Name == coffeeType);

            if (coffeeName != null)
                coffeeName.Balance += coffeePrice;

            await _db.SaveChangesAsync();
        }
    }
}