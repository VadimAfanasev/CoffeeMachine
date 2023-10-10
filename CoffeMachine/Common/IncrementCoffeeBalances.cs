using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

public class IncrementCoffeeBalances : IIncrementCoffeeBalances
{
    private readonly CoffeeContext _db;

    public IncrementCoffeeBalances(CoffeeContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Adding coffee purchased by the user to the table
    /// </summary>
    /// <param name="coffeeType"></param>
    /// <param name="coffeePrice"></param>
    /// <exception cref="Exception"></exception>
    public async Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice)
    {
        var coffeeName = await _db.Coffees.FirstOrDefaultAsync(c => c.Name == coffeeType);

        if (coffeeName != null)
            coffeeName.Balance += coffeePrice;
        else throw new Exception("Entity not found in the system");

        await _db.SaveChangesAsync();
    }
}