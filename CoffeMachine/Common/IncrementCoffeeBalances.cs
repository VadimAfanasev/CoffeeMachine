using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// A class in which we implement adding coffee purchased by the user to the database
/// </summary>
public class IncrementCoffeeBalances : IIncrementCoffeeBalances
{
    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of the class in which we implement adding purchased coffee to the database
    /// </summary>
    /// <param name="db"> </param>
    public IncrementCoffeeBalances(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice)
    {
        var coffeeName = await _db.Coffees.FirstOrDefaultAsync(c => c.Name == coffeeType);

        if (coffeeName != null)
            coffeeName.Balance += coffeePrice;
        else
            throw new Exception("Entity not found in the system");

        await _db.SaveChangesAsync();
    }
}