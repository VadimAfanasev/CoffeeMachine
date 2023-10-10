using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common;

using Microsoft.EntityFrameworkCore;

public class CalculateChange : ICalculateChange
{
    private readonly CoffeeContext _db;

    public CalculateChange(CoffeeContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Method for calculating change
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public async Task<List<uint>> CalculateAsync(uint amount)
    {
        var change = new List<uint>();

        var sortedNotes = await _db.MoneyInMachines.OrderByDescending(n => n.Nominal).ToListAsync();

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