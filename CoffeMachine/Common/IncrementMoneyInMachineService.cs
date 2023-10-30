using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// The class in which we implement adding money to the coffee machine by the administrator
/// </summary>
public class IncrementMoneyInMachineService : IIncrementMoneyInMachine
{
    /// <summary>
    /// Injecting database context
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of the class in which we implement adding money to the coffee machine
    /// </summary>
    /// <param name="db"> Context database </param>
    public IncrementMoneyInMachineService(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<string> IncrementMoneyAsync(List<MoneyDto> inputMoney)
    {
        foreach (var banknote in inputMoney.GroupBy(m => m.Nominal))
        {
            var money = await _db.MoneyInMachinesDb.FirstOrDefaultAsync(c => c.Nominal == banknote.Key);

            if (money != null)
                money.Count += Convert.ToUInt32(banknote.Sum(b => b.Count));
            else
                throw new Exception("Entity not found in the system");
        }

        await _db.SaveChangesAsync();

        return "Money deposited";
    }
}