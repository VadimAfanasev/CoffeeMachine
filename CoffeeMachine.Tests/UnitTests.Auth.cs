namespace CoffeeMachine.Tests;

[TestFixture]
public class UnitTestsAuth
{
    [Test]
    [SetUp]
    public void BuildToken_ReturnsString_WhenResultNotNull()
    {
        // Arrange
        var buildToken = new TokenService();
        string key = "MostSecretPasswordInTheWorldEver";
        string issuer = "CoffeeMachine";
        var user = new User.UserDto("Admin", "Admin");

        // Act
        var result = buildToken.BuildToken(key, issuer, user);

        // Assert
        Assert.IsNotNull(result);
    }

    [Test]
    [SetUp]
    public async Task BuildToken_ReturnsUserDto_WhenUserFound()
    {
        // Arrange
        var userRepository = new UserRepository();
        var user = new User.UserModel {UserName = "Admin", Password = "Admin"};
        var expected = new User.UserDto("Admin", "Admin");

        // Act
        var result = await userRepository.GetUserAsync(user);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}