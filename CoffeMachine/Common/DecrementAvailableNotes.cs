using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common
{
    public class DecrementAvailableNotes : IDecrementAvailableNotes
    {
        private readonly CoffeeContext _db;

        public DecrementAvailableNotes(CoffeeContext db)
        {
            _db = db;
        }

        public async Task DecrementAvailableNoteAsync(List<uint> change)
        {
            foreach (var note in change)
            {
                var money = await _db.MoneyInMachines.FirstOrDefaultAsync(c => c.Nominal == note);
                if (money != null)
                    money.Count--;
            }

            await _db.SaveChangesAsync();
        }
    }
}