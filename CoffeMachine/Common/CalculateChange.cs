using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common
{
    public class CalculateChange : ICalculateChange
    {
        private readonly CoffeeContext _db;

        public CalculateChange(CoffeeContext db)
        {
            _db = db;
        }

        public async Task<List<uint>> CalculateAsync(uint amount)
        {
            var change = new List<uint>();

            var sortedNotes = _db.MoneyInMachines.OrderByDescending(n => n.Nominal).ToList();

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
}