namespace CoffeMachine.Common
{
    using CoffeMachine.Common.Interfaces;
    using CoffeMachine.Dto;
    using CoffeMachine.Models.Data;

    public class IncrementMoneyInMachine : IIncrementMoneyInMachine
    {
        private readonly CoffeeContext _db;

        public IncrementMoneyInMachine(CoffeeContext db)
        {
            _db = db;
        }

        public void IncrementMoney(List<InputMoneyDto> inputMoney)
        {
            foreach (var banknote in inputMoney)
            {
                var money = _db.MoneyInMachines.FirstOrDefault(c => c.Nominal == banknote.Nominal);
                if (money != null)
                    money.Count = money.Count + banknote.Count;
            }

            _db.SaveChanges();
        }
    }
}