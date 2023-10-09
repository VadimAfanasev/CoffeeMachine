using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common
{
    public class IncrementMoneyInMachine : IIncrementMoneyInMachine
    {
        private readonly CoffeeContext _db;

        public IncrementMoneyInMachine(CoffeeContext db)
        {
            _db = db;
        }

        // Самостоятельно добавляем деньги в автомат. 
        public async Task IncrementMoneyAsync(List<InputMoneyDto> inputMoney)
        {
            var updateTasks = inputMoney.Select(async banknote =>
            {
                var money = await _db.MoneyInMachines.FirstOrDefaultAsync(c => c.Nominal == banknote.Nominal);
                if (money != null)
                    money.Count += banknote.Count;
            });

            await Task.WhenAll(updateTasks);

            await _db.SaveChangesAsync();
        }
    }
}