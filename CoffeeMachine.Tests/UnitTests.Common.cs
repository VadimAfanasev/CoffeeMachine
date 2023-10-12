namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsCommon
{
    [Test]
    public async Task CalculateAsyncTest_ReturnsChangeList_WhenAmountIsExact()
    {
        // Arrange
        const uint amount = 2500;
        var expected = new List<uint> { 2000u, 500u };

        var appContext = GetTestApplicationContext();
        var calculateChange = new CalculateChange(appContext);

        // Act
        var result = await calculateChange.CalculateAsync(amount);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
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
        var before—hange = appContextOld.MoneyInMachines.Sum(c => c.Count);
        var after—hange = appContext.MoneyInMachines.Sum(c => c.Count);


        Assert.That(before—hange, Is.Not.EqualTo(after—hange));
        Assert.That(before—hange - after—hange, Is.EqualTo(2));
    }

    [Test]
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
        var before—hange = appContextOld.MoneyInMachines.Sum(c => c.Count);
        var after—hange = appContext.MoneyInMachines.Sum(c => c.Count);


        Assert.That(before—hange, Is.Not.EqualTo(after—hange));
        Assert.That(after—hange - before—hange, Is.EqualTo(3));
    }











    private static CoffeeContext GetTestApplicationContext()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new CoffeeContext(contextOptions);
    }

    private static CoffeeContext GetTestApplicationContextForNothingReturnsMethods()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineTestForNothingReturnsMethods")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new CoffeeContext(contextOptions);
    }
}