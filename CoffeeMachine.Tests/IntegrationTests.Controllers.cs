

namespace CoffeeMachine.Tests;

[TestFixture]
public class IntegrationTestsControllers
{
    [Test]
    [SetUp]
    public async Task PlaceOrder_OkChange_WhenStatusCodeOk()
    {
        // Arrange
        var moneys = new uint[] { 2000, 500 };
        const string coffeeType = "Cappuccino";

        var appContext = GetTestApplicationContextNew();

        var calculateChange = new CalculateChange(appContext);
        var decrementAvailableNotes = new DecrementAvailableNotes(appContext);
        var incrementAvailableNotes = new IncrementAvailableNotes(appContext);
        var incrementCoffeeBalances = new IncrementCoffeeBalances(appContext);

        var coffeeBuyService = new CoffeeBuyServices(appContext, calculateChange, decrementAvailableNotes,
            incrementAvailableNotes,
            incrementCoffeeBalances);

        var coffeeMachineBuyController = new CoffeeMachineBuyController(coffeeBuyService);

        // Act
        var change = await coffeeMachineBuyController.PlaceOrder(coffeeType, moneys);

        // Assert
        Assert.IsNotNull(change);
        Assert.IsInstanceOf<OkObjectResult>(change.Result);
        //change.Value.Change.Should().BeEquivalentTo(expected.Change);
        //change.Should().BeEquivalentTo(expected);
        //Assert.That(change, Is.EqualTo(expected));
    }

    [Test]
    [SetUp]
    public async Task Login_OkToken_WhenStatusCodeOk()
    {
        // Arrange
        var user = new User.UserModel
        {
            UserName = "Admin",
            Password = "Admin"
        };

        var someOptions = Options.Create(new Jwt() { Key = "MostSecretPasswordInTheWorldEver", Issuer = "CoffeeMachine" });
        var tokenService = new TokenService();
        var userRepository = new UserRepository();

        var getTokenService = new GetTokenService(tokenService, userRepository, someOptions);

        var login = new CoffeeMachineLoginController(getTokenService);

        // Act
        var assert = await login.Login(user);

        // Assert
        Assert.IsNotNull(assert);
        Assert.IsInstanceOf<OkObjectResult>(assert);
    }







    private static CoffeeContext GetTestApplicationContextNew()
    {
        var contextOptions = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("CoffeeMachineUnitTestServices" + Guid.NewGuid())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var coffee—ontext = new CoffeeContext(contextOptions);

        return coffee—ontext;
    }
}