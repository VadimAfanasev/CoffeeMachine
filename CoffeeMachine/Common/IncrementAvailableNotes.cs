using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common
{
    public class IncrementAvailableNotes
    {
        private readonly ApplicationContext _db;
        public IncrementAvailableNotes(ApplicationContext db)
        {
            _db = db;
        }
        public IncrementAvailableNotes()
        {
        }

        public void IncrementAvailableNote(uint[] inputMoney)
        {
            foreach (var note in inputMoney)
            {
                var money = _db.MoneyInMachines.FirstOrDefault(c => c.Nominal == note);
                if (money != null)
                {
                    money.Count++;
                }
            }
            _db.SaveChanges();
        }
    }
}
