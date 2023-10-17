using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common;

/// <summary>
/// A class that implements methods for calculating change after buying coffee
/// </summary>
public class CalculateChange : ICalculateChange
{
    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of the class in which we calculate change
    /// </summary>
    /// <param name="db"> </param>
    public CalculateChange(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<List<uint>> CalculateAsync(uint amount)
    {
        var change = new List<uint>();

        var sortedNotes = await _db.MoneyInMachinesDb.OrderByDescending(n => n.Nominal).ToListAsync();

        foreach (var note in sortedNotes)
        {
            while (amount >= note.Nominal && note.Count > 0)
            {
                change.Add(note.Nominal);
                amount -= note.Nominal;
            }
        }

        return amount == 0 ? change : null;
    }
}