namespace CoffeeMachine.Tests;

[TestFixture]
public class IntegrationTests
{
    [Test]
    public async Task BuyingCoffeeAsync_ReturnsOrderCoffeeDto_WhenAmountIsExact()
    {
        // Arrange
        var moneys = new uint[] { 2000, 500 };
        const string coffeeType = "Cappuccino";
        var expected = new OrderCoffeeDto
        {
            Change = new List<uint> { 1000, 500, 200, 200 }
        };

        var appContext = GetTestApplicationContext();

        ICalculateChange calculateChange = new CalculateChange(appContext);
        IDecrementAvailableNotes decrementAvailableNotes = new DecrementAvailableNotes(appContext);
        IIncrementAvailableNotes incrementAvailableNotes = new IncrementAvailableNotes(appContext);
        IIncrementCoffeeBalances incrementCoffeeBalances = new IncrementCoffeeBalances(appContext);

        var coffeeBuy = new CoffeeBuyServices(appContext, calculateChange, decrementAvailableNotes, incrementAvailableNotes,
            incrementCoffeeBalances);

        // Act
        var result = await coffeeBuy.BuyingCoffeeAsync(coffeeType, moneys);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }


    [Test]
    public async Task GetBalanceCoffeeAsync_ReturnsBalanceCoffeeDto_WhenAmountIsExact()
    {
        // Arrange
        var expected = new List<BalanceCoffeeDto>
        {
            new BalanceCoffeeDto { Name = "Cappuccino", Balance = 0 },
            new BalanceCoffeeDto { Name = "Latte", Balance = 0 },
            new BalanceCoffeeDto { Name = "Americano", Balance = 0 },
            new BalanceCoffeeDto { Name = "Total", Balance = 0 }
        };

        var appContext = GetTestApplicationContext();

        var balanceCoffee = new CoffeeMachineStatusServices(appContext);

        // Act
        var result = await balanceCoffee.GetBalanceCoffeeAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetBalanceMoneyAsync_ReturnsMoneyDto_WhenAmountIsExact()
    {
        // Arrange
        var expected = new List<MoneyDto>
        {

            new MoneyDto() { Nominal = 50, Count = 10 },
            new MoneyDto() { Nominal = 100, Count = 10 },
            new MoneyDto() { Nominal = 200, Count = 10 },
            new MoneyDto() { Nominal = 500, Count = 10 },
            new MoneyDto() { Nominal = 1000, Count = 10 },
            new MoneyDto() { Nominal = 2000, Count = 10 },
            new MoneyDto() { Nominal = 5000, Count = 10 }
        };

        var appContext = GetTestApplicationContext();

        var balanceCoffee = new CoffeeMachineStatusServices(appContext);

        // Act
        var result = await balanceCoffee.GetBalanceMoneyAsync();

        // Assert
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