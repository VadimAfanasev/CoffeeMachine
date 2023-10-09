using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Common
{
    public class IncrementAvailableNotes : IIncrementAvailableNotes
    {
        private readonly CoffeeContext _db;

        public IncrementAvailableNotes(CoffeeContext db)
        {
            _db = db;
        }

        // Добавляем внесенные пользователем деньги в таблицу
        public async Task IncrementAvailableNoteAsync(uint[] inputMoney)
        {
            foreach (var note in inputMoney)
            {
                var money = await _db.MoneyInMachines.FirstOrDefaultAsync(c => c.Nominal == note);
                if (money != null)
                    money.Count++;
            }

            await _db.SaveChangesAsync();
        }
    }
}