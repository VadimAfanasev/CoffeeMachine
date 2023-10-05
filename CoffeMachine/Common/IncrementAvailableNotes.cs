using CoffeMachine.Common.Interfaces;
using CoffeMachine.Models;
using CoffeMachine.Models.Data;

namespace CoffeMachine.Common
{
    public class IncrementAvailableNotes: IIncrementAvailableNotes
    {
        private readonly CoffeeContext _db;
        public IncrementAvailableNotes(CoffeeContext db)
        {
            _db = db;
        }

        public void IncrementAvailableNote(uint[] inputMoney)
        {
            foreach (var note in inputMoney)
            {
                var money = _db.MoneyInMachines.FirstOrDefault(c=>c.Nominal == note);
                if (money != null)
                {
                    money.Count++;
                }
            }
            _db.SaveChanges();
        }
    }
}