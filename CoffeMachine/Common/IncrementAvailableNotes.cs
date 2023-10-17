using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// The class in which we implement adding money contributed by the user to the database
/// </summary>
public class IncrementAvailableNotes : IIncrementAvailableNotes
{
    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of the class in which we implement adding money to the database
    /// </summary>
    /// <param name="db"> </param>
    public IncrementAvailableNotes(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task IncrementAvailableNoteAsync(uint[] inputMoney)
    {
        foreach (var note in inputMoney)
        {
            var money = await _db.MoneyInMachinesDb.FirstOrDefaultAsync(c => c.Nominal == note);
            if (money != null)
                money.Count++;
            else
                throw new Exception("Entity not found in the system");
        }

        await _db.SaveChangesAsync();
    }
}