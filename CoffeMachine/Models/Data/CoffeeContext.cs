using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Models.Data;

/// <summary>
/// The class in which we add the database context
/// </summary>
public class CoffeeContext : DbContext
{
    /// <summary>
    /// Create a class constructor with parameters
    /// </summary>
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
            Coffees.AddRange(coffeePrice.Select(s => new Coffee
            {
                Name = s.Key,
                Price = s.Value,
                Balance = 0
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
            MoneyInMachines.AddRange(availableNotes.Select(c => new MoneyInMachine
            {
                Nominal = c.Key,
                Count = c.Value
            }));
        }

        SaveChanges();
    }

    /// <summary>
    /// Creating a coffee table in a coffee machine
    /// </summary>
    public virtual DbSet<Coffee> Coffees { get; set; }

    /// <summary>
    /// Creating a money table in a coffee machine
    /// </summary>
    public virtual DbSet<MoneyInMachine> MoneyInMachines { get; set; }
}