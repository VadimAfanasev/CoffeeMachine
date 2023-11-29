using CoffeeMachine.Common.Constants;
using CoffeeMachine.Common.Enums;
using CoffeeMachine.Models;

namespace CoffeeMachine.Tests.Infrastructure;

public class TestDbBaseContext
{
    [SetUp]
    public static CoffeeContext GetTestInitAppContext(CoffeeContext context = null)
    {
        var dbTest = context ?? GetTestApplicationContextNew();

        var coffeePrice = new Dictionary<string, uint>
        {
            { CoffeeNames.CAPPUCCINO, 600 },
            { CoffeeNames.LATTE, 850 },
            { CoffeeNames.AMERICANO, 900 }
        };
        dbTest.CoffeesDb.AddRange(coffeePrice.Select(s => new Coffee
        {
            Name = s.Key,
            Price = s.Value,
            Balance = 0
        }));

        var availableNotes = new Dictionary<uint, uint>
        {
            { (uint)InputAdminBanknotesEnums.Fifty, 10u },
            { (uint)InputAdminBanknotesEnums.OneHundred, 10u },
            { (uint)InputAdminBanknotesEnums.TwoHundred, 10u },
            { (uint)InputAdminBanknotesEnums.FiveHundred, 10u },
            { (uint)InputAdminBanknotesEnums.OneThousand, 10u },
            { (uint)InputAdminBanknotesEnums.TwoThousand, 10u },
            { (uint)InputAdminBanknotesEnums.FiveThousand, 10u }
        };
        dbTest.MoneyInMachinesDb.AddRange(availableNotes.Select(c => new MoneyInMachine
        {
            Nominal = c.Key,
            Count = c.Value
        }));

        dbTest.SaveChanges();

        return dbTest;
    }

    internal static CoffeeContext GetTestApplicationContextNew()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineUnitTestServices" + Guid.NewGuid())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var coffeeСontext = new CoffeeContext(contextOptions);
        coffeeСontext.Database.EnsureCreated();

        return coffeeСontext;
    }
}