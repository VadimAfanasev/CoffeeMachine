using CoffeeMachine.Common.Enums;
using CoffeeMachine.Tests.Infrastructure;
using LazyCache;
using Moq;

namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsServices
{
    [Test]
    [SetUp]
    public async Task BuyingCoffeeAsync_ReturnsOrderCoffeeDto_WhenAmountIsExact()
    {
        // Arrange 
        var moneys = new[] { (uint)InputBuyerBanknotesEnums.TwoThousand, (uint)InputBuyerBanknotesEnums.FiveHundred };
        const string coffeeType = "Cappuccino";
        var expected = new OrderCoffeeDto
        {
            Change = new List<uint> { 1000, 500, 200, 200 }
        };

        var appContext = GetTestApplicationContextNew();

        var cacheMock = new Mock<IAppCache>();
        var calculateChange = new ChangeCalculationService(appContext);
        var moneyService = new DepositService(appContext);

        var coffeeBuyService = new CoffeeBuyServices(appContext, moneyService, calculateChange, cacheMock.Object);

        // Act
        var result = await coffeeBuyService.BuyingCoffeeAsync(coffeeType, moneys);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }


    [Test]
    [SetUp]
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

        var appContext = GetTestApplicationContextNew();
        var cacheMock = new Mock<IAppCache>();

        var balanceCoffeeService = new CoffeeMachineStatusServices(appContext, cacheMock.Object);

        // Act
        var result = await balanceCoffeeService.GetBalanceCoffeeAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    [SetUp]
    public async Task GetBalanceMoneyAsync_ReturnsMoneyDto_WhenAmountIsExact()
    {
        // Arrange
        var expected = new List<MoneyDto>();

        foreach (InputAdminBanknotesEnums nominal in Enum.GetValues(typeof(InputAdminBanknotesEnums)))
        {
            var moneyDto = new MoneyDto { Count = 10, Nominal = (uint)nominal };
            expected.Add(moneyDto);
        }

        var appContext = GetTestApplicationContextNew();
        var cacheMock = new Mock<IAppCache>();

        var balanceCoffeeService = new CoffeeMachineStatusServices(appContext, cacheMock.Object);

        // Act
        var result = await balanceCoffeeService.GetBalanceMoneyAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    [SetUp]
    public async Task InputingAsync_ReturnsString_WhenContentAreEqual()
    {
        // Arrange
        const string expected = "Money deposited";
        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = (uint)InputAdminBanknotesEnums.Fifty, Count = 2 },
            new MoneyDto() { Nominal = (uint)InputAdminBanknotesEnums.OneHundred, Count = 2 },
        };

        var appContext = GetTestApplicationContextNew();
        var incrementMoneyInMachine = new IncrementMoneyInMachineService(appContext);
        var cacheMock = new Mock<IAppCache>();

        var balanceCoffeeService = new InputMoneyServices(appContext, incrementMoneyInMachine, cacheMock.Object);

        // Act
        var result = await balanceCoffeeService.InputingAsync(inputMoney);

        // Assert
        expected.Should().BeEquivalentTo(result);
    }

    private static CoffeeContext GetTestApplicationContextNew()
    {
        return TestDbBaseContext.GetTestInitAppContext();
    }
}