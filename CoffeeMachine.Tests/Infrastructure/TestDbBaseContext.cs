using CoffeeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Tests.Infrastructure
{
    internal class TestDbBaseContext
    {
        internal static CoffeeContext GetTestInitAppContext()
        {
            var dbTest = GetTestApplicationContextNew();

            var coffeePrice = new Dictionary<string, uint>
        {
            { "Cappuccino", 600 },
            { "Latte", 850 },
            { "Americano", 900 }
        };
            dbTest.CoffeesDb.AddRange(coffeePrice.Select(s => new Coffee
            {
                Name = s.Key,
                Price = s.Value,
                Balance = 0
            }));

            var availableNotes = new Dictionary<uint, uint>
        {
            { 50, 10 },
            { 100, 10 },
            { 200, 10},
            { 500, 10 },
            { 1000, 10 },
            { 2000, 10 },
            { 5000, 10 }
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
}
