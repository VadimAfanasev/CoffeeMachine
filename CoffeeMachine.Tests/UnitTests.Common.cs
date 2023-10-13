namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsCommon
{
    [Test]
    [SetUp]
    public async Task CalculateAsyncTest_ReturnsChangeList_WhenAmountIsExact()
    {
        // Arrange
        const uint amount = 2500;
        var expected = new List<uint> { 2000u, 500u };

        var appContext = GetTestApplicationContextNew();
        var calculateChange = new CalculateChange(appContext);

        // Act
        var result = await calculateChange.CalculateAsync(amount);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    [SetUp]
    public async Task IncrementMoneyAsync_ReturnsString_WhenBalanceMoneyNotEqual()
    {
        // Arrange

        const string expected = "Money deposited";
        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = 50, Count = 2 },
            new MoneyDto() { Nominal = 100, Count = 2 },
        };

        var appContext = GetTestApplicationContextNew();
        var incrementMoneyInMachine = new IncrementMoneyInMachine(appContext);

        // Act
        var result = await incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);

        // Assert
        expected.Should().BeEquivalentTo(result);
    }

    private static CoffeeContext GetTestApplicationContextNew()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineUnitTestServices" + Guid.NewGuid().ToString())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var coffeeÑontext = new CoffeeContext(contextOptions);

        return coffeeÑontext;
    }
}