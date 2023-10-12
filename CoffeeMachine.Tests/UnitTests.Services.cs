namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsServices
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

        var balanceCoffeeService = new CoffeeMachineStatusServices(appContext);

        // Act
        var result = await balanceCoffeeService.GetBalanceCoffeeAsync();

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

        var balanceCoffeeService = new CoffeeMachineStatusServices(appContext);

        // Act
        var result = await balanceCoffeeService.GetBalanceMoneyAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task InputingAsync_ReturnsString_WhenContentAreEqual()
    {
        // Arrange
        var expected = "Money deposited";
        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = 50, Count = 2 },
            new MoneyDto() { Nominal = 100, Count = 2 },
        };

        var appContext = GetTestApplicationContext();
        var incrementMoneyInMachine = new IncrementMoneyInMachine(appContext);

        var balanceCoffeeService = new InputMoneyServices(appContext, incrementMoneyInMachine);

        // Act
        var result = await balanceCoffeeService.InputingAsync(inputMoney);

        // Assert
        expected.Should().BeEquivalentTo(result);
    }

    [Test]
    public async Task GetTokenAsync_ReturnsString_WhenUserAreEquals()
    {
        // Arrange    
        var user = new UserModel
        {
            UserName = "Admin",
            Password = "Admin"
        };

        //IOptions<Jwt> someOptions = Options.Create<Jwt>(new Jwt());
        var someOptions = Options.Create(new Jwt(){Key = "MostSecretPasswordInTheWorldEver",Issuer = "CoffeeMachine" });
        var tokenService = new TokenService();
        var userRepository = new UserRepository();

        var getToken = new GetTokenService(tokenService, userRepository, someOptions);

        // Act    
        var result = await getToken.GetTokenAsync(user);

        // Assert
        Assert.IsNotNull(result);
    }

    private static CoffeeContext GetTestApplicationContext()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var coffee—ontext = new CoffeeContext(contextOptions);
        

        return coffee—ontext;
    }
}