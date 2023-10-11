namespace CoffeeMachine.Tests;

[TestFixture]
public class Tests
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





    private static CoffeeContext GetTestApplicationContext()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new CoffeeContext(contextOptions);
    }
}