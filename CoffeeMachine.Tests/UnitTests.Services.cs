using CoffeeMachine.Tests.Infrastructure;

namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsServices
{
    [Test]
    [SetUp]
    public async Task BuyingCoffeeAsync_ReturnsOrderCoffeeDto_WhenAmountIsExact()
    {
        // Arrange 
        var moneys = new uint[] { 2000, 500 };
        const string coffeeType = "Cappuccino";
        var expected = new OrderCoffeeDto
        {
            Change = new List<uint> { 1000, 500, 200, 200 }
        };

        var appContext = GetTestApplicationContextNew();


        var calculateChange = new CalculateChange(appContext);
        var decrementAvailableNotes = new DecrementAvailableNotes(appContext);
        var incrementAvailableNotes = new IncrementAvailableNotes(appContext);
        var incrementCoffeeBalances = new IncrementCoffeeBalances(appContext);

        var coffeeBuyService = new CoffeeBuyServices(appContext, calculateChange, decrementAvailableNotes,
            incrementAvailableNotes,
            incrementCoffeeBalances);

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

        var balanceCoffeeService = new CoffeeMachineStatusServices(appContext);

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

        var appContext = GetTestApplicationContextNew();

        var balanceCoffeeService = new CoffeeMachineStatusServices(appContext);

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
            new MoneyDto() { Nominal = 50, Count = 2 },
            new MoneyDto() { Nominal = 100, Count = 2 },
        };

        var appContext = GetTestApplicationContextNew();
        var incrementMoneyInMachine = new IncrementMoneyInMachine(appContext);

        var balanceCoffeeService = new InputMoneyServices(appContext, incrementMoneyInMachine);

        // Act
        var result = await balanceCoffeeService.InputingAsync(inputMoney);

        // Assert
        expected.Should().BeEquivalentTo(result);
    }

    [Test]
    [SetUp]
    public async Task GetTokenAsync_ReturnsString_WhenNotNull()
    {
        // Arrange    
        var user = new User.UserModel
        {
            UserName = "Admin",
            Password = "Admin"
        };

        var someOptions = Options.Create(new Jwt(){Key = "MostSecretPasswordInTheWorldEver",Issuer = "CoffeeMachine" });
        var tokenService = new TokenService();
        var userRepository = new UserRepository();

        var getToken = new GetTokenService(tokenService, userRepository, someOptions);

        // Act    
        var result = await getToken.GetTokenAsync(user);

        // Assert
        Assert.IsNotNull(result);
    }

    private static CoffeeContext GetTestApplicationContextNew()
    {
        return TestDbBaseContext.GetTestInitAppContext();
    }
}