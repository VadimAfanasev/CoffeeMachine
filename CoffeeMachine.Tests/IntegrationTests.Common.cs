namespace CoffeeMachine.Tests;

[TestFixture]
public class IntegrationTestsCommon
{
    [Test]
    [SetUp]
    public async Task DecrementAvailableNoteAsync_NothingReturns_WhenCountMoneyNotEqual()
    {
        // Arrange
        var money = new List<uint> { 2000u, 500u };

        var appContext = GetTestApplicationContext();
        var appContextOld = GetTestApplicationContextForNothingReturnsMethods();
        var decrementAvailableNotes = new DecrementAvailableNotes(appContext);

        // Act
        await decrementAvailableNotes.DecrementAvailableNoteAsync(money);

        // Assert
        var before�hange = appContextOld.MoneyInMachines.Sum(c => c.Count);
        var after�hange = appContext.MoneyInMachines.Sum(c => c.Count);

        Assert.That(before�hange, Is.Not.EqualTo(after�hange));
        Assert.That(before�hange - after�hange, Is.EqualTo(2));
    }

    [Test]
    [SetUp]
    public async Task IncrementAvailableNoteAsync_NothingReturns_WhenCountMoneyNotEqual()
    {
        // Arrange
        var money = new uint[] { 2000u, 1000u, 500u };

        var appContext = GetTestApplicationContext();
        var appContextOld = GetTestApplicationContextForNothingReturnsMethods();
        var incrementAvailableNotes = new IncrementAvailableNotes(appContext);

        // Act
        await incrementAvailableNotes.IncrementAvailableNoteAsync(money);

        // Assert
        var before�hange = appContextOld.MoneyInMachines.Sum(c => c.Count);
        var after�hange = appContext.MoneyInMachines.Sum(c => c.Count);

        Assert.That(before�hange, Is.Not.EqualTo(after�hange));
        Assert.That(after�hange - before�hange, Is.EqualTo(3));
    }

    [Test]
    [SetUp]
    public async Task IncrementCoffeeBalanceAsync_NothingReturns_WhenBalanceCoffeeNotEqual()
    {
        // Arrange

        const string coffeeName = "Latte";
        const uint coffeePrice = 850u;
        var appContext = GetTestApplicationContext();
        var appContextOld = GetTestApplicationContextForNothingReturnsMethods();
        var incrementCoffeeBalance = new IncrementCoffeeBalances(appContext);

        // Act
        await incrementCoffeeBalance.IncrementCoffeeBalanceAsync(coffeeName, coffeePrice);

        // Assert
        var before�hange = appContextOld.Coffees.Sum(c => c.Balance);
        var after�hange = appContext.Coffees.Sum(c => c.Balance);

        Assert.That(before�hange, Is.Not.EqualTo(after�hange));
        Assert.That(after�hange - before�hange, Is.EqualTo(850u));
    }
    private static CoffeeContext GetTestApplicationContext()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineUnitTestServices" + Guid.NewGuid().ToString())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var coffee�ontext = new CoffeeContext(contextOptions);

        return coffee�ontext;
    }

    private static CoffeeContext GetTestApplicationContextForNothingReturnsMethods()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineUnitTestServices" + Guid.NewGuid().ToString())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var coffee�ontext = new CoffeeContext(contextOptions);

        return coffee�ontext;
    }
}