using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// The class in which we implement adding money to the coffee machine by the administrator
/// </summary>
public class IncrementMoneyInMachine : IIncrementMoneyInMachine
{
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of the class in which we implement adding money to the coffee machine
    /// </summary>
    /// <param name="db"> </param>
    public IncrementMoneyInMachine(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task IncrementMoneyAsync(List<MoneyDto> inputMoney)
    {
        var updateTasks = inputMoney.Select(async banknote =>
        {
            var money = await _db.MoneyInMachines.FirstOrDefaultAsync(c => c.Nominal == banknote.Nominal);
            if (money != null)
                money.Count += banknote.Count;
            else
                throw new Exception("Entity not found in the system");
        });

        await Task.WhenAll(updateTasks);

        await _db.SaveChangesAsync();
    }
}