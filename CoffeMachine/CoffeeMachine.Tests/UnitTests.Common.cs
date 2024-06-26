using CoffeeMachine.Tests.Infrastructure;

namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsCommon
{
    /// <summary>
    /// Checking the change calculated by the algorithm
    /// </summary>
    /// <returns> result equivalent expected </returns>
    [Test]
    [SetUp]
    public async Task CalculateAsyncTest_ReturnsChangeList_WhenAmountIsExact()
    {
        // Arrange
        const uint amount = 2500;
        var expected = new List<uint> { 2000u, 500u };

        var appContext = GetTestInitAppContext();
        var calculateChange = new ChangeCalculationService(appContext);

        // Act
        var result = await calculateChange.CalculateAsync(amount);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Checking whether money is being deposited into the account
    /// </summary>
    /// <returns> Money deposited </returns>
    [Test]
    [SetUp]
    public async Task IncrementMoneyAsync_ReturnsString_WhenMoneyEntered()
    {
        // Arrange

        const string expected = "Money deposited";
        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = 50, Count = 2 },
            new MoneyDto() { Nominal = 100, Count = 2 },
        };

        var appContext = GetTestInitAppContext();
        var incrementMoneyInMachine = new IncrementMoneyInMachineService(appContext);

        // Act
        var result = await incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);

        // Assert
        expected.Should().BeEquivalentTo(result);
    }


    private static CoffeeContext GetTestInitAppContext()
    {
        return TestDbBaseContext.GetTestInitAppContext();
    }
}