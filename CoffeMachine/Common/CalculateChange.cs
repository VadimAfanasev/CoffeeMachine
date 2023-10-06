namespace CoffeMachine.Common
{
    using CoffeMachine.Common.Interfaces;
    using CoffeMachine.Models.Data;

    public class CalculateChange : ICalculateChange
    {
        private readonly CoffeeContext _db;

        public CalculateChange(CoffeeContext db)
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