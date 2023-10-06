namespace CoffeMachine.Common
{
    using CoffeMachine.Common.Interfaces;
    using CoffeMachine.Models.Data;

    public class DecrementAvailableNotes : IDecrementAvailableNotes
    {
        private readonly CoffeeContext _db;

        public DecrementAvailableNotes(CoffeeContext db)
        {
            _db = db;
        }

        public void DecrementAvailableNote(List<uint> change)
        {
            foreach (var note in change)
            {
                var money = _db.MoneyInMachines.FirstOrDefault(c => c.Nominal == note);
                if (money != null)
                    money.Count--;
            }

            _db.SaveChanges();
        }
    }
}