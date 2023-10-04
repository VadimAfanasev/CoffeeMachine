using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common
{
    public class DecrementAvailableNotes
    {
        private readonly ApplicationContext _db;
        public DecrementAvailableNotes(ApplicationContext db)
        {
            _db = db;
        }
        public DecrementAvailableNotes()
        {
        }

        public void DecrementAvailableNote(List<uint> change)
        {
            foreach (var note in change)
            {
                var money = _db.MoneyInMachines.FirstOrDefault(c => c.Nominal == note);
                if (money != null)
                {
                    money.Count--;
                }
            }
            _db.SaveChanges();
        }
    }
}
