using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common
{
    using Microsoft.EntityFrameworkCore;

    public class CalculateChange : ICalculateChange
    {
        private readonly CoffeeContext _db;

        public CalculateChange(CoffeeContext db)
        {
            _db = db;
        }

        // Метод для вычисления сдачи
        public async Task<List<uint>> CalculateAsync(uint amount)
        {
            var change = new List<uint>();

            var sortedNotes = await _db.MoneyInMachines.OrderByDescending(n => n.Nominal).ToListAsync();

            // цикл в котором реализован жадный алгоритм для вычисления оптимальной сдачи
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