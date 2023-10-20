using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// Class for dealing with money when buying coffee
/// </summary>
public class DepositService: IDepositService
{
    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;


    /// <summary>
    /// Class constructor for dealing with money when buying coffee
    /// </summary>
    /// <param name="db"> Context database </param>
    public DepositService(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task IncrementAvailableNotesAsync(uint[] inputMoney)
    {
        var allMoney = await _db.MoneyInMachinesDb.ToListAsync();

        foreach (var moneyInMachine in inputMoney.Select(note => allMoney
                         .FirstOrDefault(m => m.Nominal == note))
                     .Where(moneyInMachine => moneyInMachine != null))
        {
            moneyInMachine.Count++;
        }

        await _db.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice)
    {
        var coffeeName = await _db.CoffeesDb.FirstOrDefaultAsync(c => c.Name == coffeeType);

        if (coffeeName != null)
            coffeeName.Balance += coffeePrice;
        else
            throw new Exception("No coffee with this name in the system");
    }
}