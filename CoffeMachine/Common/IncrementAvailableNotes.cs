using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

public class IncrementAvailableNotes : IIncrementAvailableNotes
{
    private readonly CoffeeContext _db;

    public IncrementAvailableNotes(CoffeeContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Adding money contributed by the user to the table
    /// </summary>
    /// <param name="inputMoney"></param>
    /// <exception cref="Exception"></exception>
    public async Task IncrementAvailableNoteAsync(uint[] inputMoney)
    {
        foreach (var note in inputMoney)
        {
            var money = await _db.MoneyInMachines.FirstOrDefaultAsync(c => c.Nominal == note);
            if (money != null)
                money.Count++;
            else throw new Exception("Entity not found in the system");
        }

        await _db.SaveChangesAsync();
    }
}