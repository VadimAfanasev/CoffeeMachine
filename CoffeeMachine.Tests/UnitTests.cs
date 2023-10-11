using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Services;

using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoffeeMachine.Tests;

[TestFixture()]
public class Tests
{
    [Test]
    public async Task CalculateAsyncTest_ReturnsChangeList_WhenAmountIsExact()
    {
        // Arrange
        const uint amount = 2500;
        var appContext = GetTestApplicationContext();
        var calculateChange = new CalculateChange(appContext);

        // Act
        var result = await calculateChange.CalculateAsync(amount);

        // Assert
        var expected = new List<uint> { 2000u, 500u };
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task BuyingCoffeeAsync()
    {
        // Arrange
        var moneys = new uint[] { 2000, 500 };
        const string coffeeType = "Cappuccino";
        var appContext = GetTestApplicationContext();
        //var calculateChangeMock = new Mock<ICalculateChange>();
        //var decrementAvailableNotesMock = new Mock<IDecrementAvailableNotes>();
        //var incrementAvailableNotesMock = new Mock<IIncrementAvailableNotes>();
        //var incrementCoffeeBalancesMock = new Mock<IIncrementCoffeeBalances>();
        ICalculateChange calculateChange = new CalculateChange(appContext);
        IDecrementAvailableNotes decrementAvailableNotes = new DecrementAvailableNotes(appContext);
        IIncrementAvailableNotes incrementAvailableNotes = new IncrementAvailableNotes(appContext);
        IIncrementCoffeeBalances incrementCoffeeBalances = new IncrementCoffeeBalances(appContext);

        var coffeeBuy = new CoffeeBuyServices(appContext, calculateChange, decrementAvailableNotes, incrementAvailableNotes,
            incrementCoffeeBalances);

        // Act
        var result = await coffeeBuy.BuyingCoffeeAsync(coffeeType, moneys);

        // Assert
        var expected = new OrderCoffeeDto
        {
            Change = new List<uint> { 1000, 500, 200, 200 }
        };
        result.Should().BeEquivalentTo(expected);
    }

    private static CoffeeContext GetTestApplicationContext()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new CoffeeContext(contextOptions);
    }
}