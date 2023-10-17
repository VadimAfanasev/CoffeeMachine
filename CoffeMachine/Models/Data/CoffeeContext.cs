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
        if (Database.IsRelational())
        {
            Database.Migrate();
        }

        SaveChanges();
    }

    /// <summary>
    /// Creating a coffee table in a coffee machine
    /// </summary>
    public virtual DbSet<Coffee> CoffeesDb { get; set; }

    /// <summary>
    /// Creating a money table in a coffee machine
    /// </summary>
    public virtual DbSet<MoneyInMachine> MoneyInMachinesDb { get; set; }
}