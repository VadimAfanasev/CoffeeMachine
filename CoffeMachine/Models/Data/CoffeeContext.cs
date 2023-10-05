using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CoffeMachine.Models.Data
{
    public class CoffeeContext : DbContext
    {
        public virtual DbSet<MoneyInMachine> MoneyInMachines { get; set; }
        public virtual DbSet<Coffee> Coffees { get; set; }
        public virtual DbSet<CoffeeBalance> CoffeeBalances { get; set; }

        public CoffeeContext(DbContextOptions<CoffeeContext> options) : base(options)
        {
            Database.EnsureCreated();
            if (!Coffees.Any())
            {
                var coffeePrice = new Dictionary<string, uint>
                {
                    { "Cappuccino", 600 },
                    { "Latte", 850 },
                    { "Americano", 900 }
                }; 
                Coffees.AddRange(coffeePrice.Select(s => new Coffee() {
                    Name = s.Key,
                    Price = s.Value
                }));
            }

            if (!MoneyInMachines.Any())
            {
                var availableNotes = new Dictionary<uint, uint>
                {
                    { 50, 10 },
                    { 100, 10 },
                    { 200, 10 },
                    { 500, 10 },
                    { 1000, 10 },
                    { 2000, 10 },
                    { 5000, 10 }
                };
                MoneyInMachines.AddRange(availableNotes.Select(c => new MoneyInMachine()
                {
                    Nominal = c.Key,
                    Count = c.Value
                }));
            }

            if (!CoffeeBalances.Any())
            {
                var coffee = new Dictionary<string, uint>
                {
                    { "Cappuccino", 0 },
                    { "Latte", 0 },
                    { "Americano", 0 }
                };
                CoffeeBalances.AddRange(coffee.Select(c => new CoffeeBalance()
                {
                    Name = c.Key,
                    Balance = c.Value
                }));
            }
            SaveChanges();
        }
    }
}
