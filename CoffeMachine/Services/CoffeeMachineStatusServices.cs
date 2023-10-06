namespace CoffeMachine.Services
{
    using CoffeMachine.Dto;
    using CoffeMachine.Models.Data;
    using CoffeMachine.Services.Interfaces;

    public class CoffeeMachineStatusServices : ICoffeeMachineStatusServices
    {
        private readonly CoffeeContext _db;

        public CoffeeMachineStatusServices(CoffeeContext db)
        {
            _db = db;
        }

        public List<BalanceCoffeeDto> GetBalanceCoffee()
        {
            var balanceCoffee = new List<BalanceCoffeeDto>();
            var coffeeBalances = _db.CoffeeBalances.ToList();
            uint totalBalance = 0;

            foreach (var balance in coffeeBalances)
            {
                var balanceDto = new BalanceCoffeeDto
                {
                    Name = balance.Name,
                    Balance = balance.Balance
                };
                balanceCoffee.Add(balanceDto);
                totalBalance += balance.Balance;
            }

            var totalBalanceDto = new BalanceCoffeeDto
            {
                Name = "Total",
                Balance = totalBalance
            };

            balanceCoffee.Add(totalBalanceDto);

            if (balanceCoffee == null)
            {
                throw new Exception("Entity not found in the system");
            }

            return balanceCoffee;
        }

        public List<BalanceMoneyDto> GetBalanceMoney()
        {
            var balanceMoney = new List<BalanceMoneyDto>();
            var moneyBalances = _db.MoneyInMachines.ToList();

            foreach (var balance in moneyBalances)
            {
                var balanceDto = new BalanceMoneyDto
                {
                    Nominal = balance.Nominal,
                    Count = balance.Count
                };
                balanceMoney.Add(balanceDto);
            }

            var sortedBalanceMoney = balanceMoney.OrderByDescending(n => n.Nominal).ToList();

            if (sortedBalanceMoney==null)
            {
                throw new Exception("Entity not found in the system");
            }

            return sortedBalanceMoney;
        }
    }
}