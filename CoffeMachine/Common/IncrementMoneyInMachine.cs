using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

public class IncrementMoneyInMachine : IIncrementMoneyInMachine
{
    private readonly CoffeeContext _db;

    public IncrementMoneyInMachine(CoffeeContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Add money to the machine yourself
    /// </summary>
    /// <param name="inputMoney"></param>
    /// <exception cref="Exception"></exception>
    public async Task IncrementMoneyAsync(List<InputMoneyDto> inputMoney)
    {
        var updateTasks = inputMoney.Select(async banknote =>
        {
            var money = await _db.MoneyInMachines.FirstOrDefaultAsync(c => c.Nominal == banknote.Nominal);
            if (money != null)
                money.Count += banknote.Count;
            else throw new Exception("Entity not found in the system");
        });

        await Task.WhenAll(updateTasks);

        await _db.SaveChangesAsync();
    }
}