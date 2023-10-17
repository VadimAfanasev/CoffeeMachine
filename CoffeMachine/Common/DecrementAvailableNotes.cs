using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// A class in which banknotes issued as change are subtracted from the database
/// </summary>
public class DecrementAvailableNotes : IDecrementAvailableNotes
{
    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of the class in which issued banknotes are subtracted from the database
    /// </summary>
    /// <param name="db"> </param>
    public DecrementAvailableNotes(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task DecrementAvailableNoteAsync(List<uint> change)
    {
        foreach (var note in change)
        {
            var money = await _db.MoneyInMachinesDb.FirstOrDefaultAsync(c => c.Nominal == note);
            if (money != null)
                money.Count--;
            else
                throw new Exception("Entity not found in the system");
        }

        await _db.SaveChangesAsync();
    }
}