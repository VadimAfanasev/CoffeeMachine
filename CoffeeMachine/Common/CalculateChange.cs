using CoffeeMachine.Models.Data;

namespace CoffeeMachine.Common
{
    public class CalculateChange
    {
        private readonly ApplicationContext _db;
        public CalculateChange()
        {
        }
        public CalculateChange(ApplicationContext db)
        {
            _db = db;
        }

        public List<uint> Calculate(uint amount)
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
