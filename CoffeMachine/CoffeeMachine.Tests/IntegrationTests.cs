using Bogus;
using CoffeeMachine.Common.Constants;
using CoffeeMachine.Common.Enums;
using CoffeeMachine.Tests.Infrastructure;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CoffeeMachine.Tests;

[TestFixture]
public class IntegrationTests : CustomBaseTest
{
    /// <summary>
    /// We simulate the purchase of coffee, compare the given change with the calculated change.
    /// We check the functionality of the controller and whether the change was issued correctly
    /// </summary>
    /// <returns>Change DTO</returns>
    [Test]
    public async Task OrderCoffeeTest_ReturnsOrderCoffeeDto_WhenDtoAreEqual()
    {
        // Arrange
        var inputMoney = new uint[] { (uint)InputBuyerBanknotesEnums.TwoThousand, (uint)InputBuyerBanknotesEnums.FiveHundred };
        const string coffeeName = CoffeeNames.CAPPUCCINO;
        var expected = new OrderCoffeeDto { Change = new List<uint> { 1000, 500, 200, 200 } };

        var client = GetClient();
        var content = JsonContent.Create(inputMoney);

        // Act
        var response = await client.PostAsync($"api/order/{coffeeName}", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<OrderCoffeeDto>(responseContent)!;

        // Actual
        actual.Should().BeEquivalentTo(expected);
        Assert.That(actual.Change, Is.EquivalentTo(expected.Change));
    }

    /// <summary>
    /// We simulate the purchase of coffee, check how the controller works if you enter the wrong name of coffee
    /// </summary>
    /// <returns>Error</returns>
    [Test]
    public async Task OrderCoffeeTest_ReturnsException_InvalidCoffeeName()
    {
        // Arrange
        var inputMoney = new uint[] { (uint)InputBuyerBanknotesEnums.TwoThousand, (uint)InputBuyerBanknotesEnums.FiveHundred };
        const string invalidCoffeeName = "InvalidCoffeeName";

        var client = GetClient();
        var content = JsonContent.Create(inputMoney);

        // Act
        var response = await client.PostAsync($"api/order/{invalidCoffeeName}", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().Contain("Invalid coffee type");
    }

    /// <summary>
    /// We simulate the purchase of coffee, check how the controller works if you enter the wrong amount of money
    /// </summary>
    /// <returns>Exception</returns>
    [Test]
    public async Task OrderCoffeeTest_ReturnsOrderCoffeeDto_InvalidMoneyAmount()
    {
        // Arrange
        var inputMoney = new uint[] { (uint)InputBuyerBanknotesEnums.FiveHundred };
        const string coffeeName = CoffeeNames.CAPPUCCINO;

        var client = GetClient();
        var content = JsonContent.Create(inputMoney);

        // Act
        var response = await client.PostAsync($"api/order/{coffeeName}", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().Contain("The amount deposited is less than required");
    }

    /// <summary>
    /// We simulate multiple purchases of coffee, check how the algorithm for calculating change works
    /// </summary>
    /// <returns> Cannot provide change </returns>
    [Test]
    public async Task OrderCoffeeTest_ReturnsOrderCoffeeDto_ManyOrdersCheckDb()
    {
        // Arrange
        const string coffeeName = CoffeeNames.CAPPUCCINO;
        var response = new HttpResponseMessage();
        var balanceDatabase = "";

        var endlessСycle = true;
        var iteration = 0;

        var client = GetClient();

        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = 5000, Count = 100 },
            new MoneyDto() { Nominal = 2000, Count = 100 },
            new MoneyDto() { Nominal = 1000, Count = 100 },
            new MoneyDto() { Nominal = 500, Count = 100 },
            new MoneyDto() { Nominal = 200, Count = 100 },
            new MoneyDto() { Nominal = 100, Count = 100 },
            new MoneyDto() { Nominal = 50, Count = 100 }
        };
        var inputMoneyJson = JsonContent.Create(inputMoney);
        await client.PutAsync("api/inputing", inputMoneyJson);

        // Act
        while (endlessСycle)
        {
            var generator = new RandomCombinationGenerator();
            var combination = generator.GenerateRandomCombination();
            var content = JsonContent.Create(combination);

            iteration++;
            if (iteration == 100)
                endlessСycle = false;
            response = await client.PostAsync($"api/order/{coffeeName}", content);
            var balance = await client.GetAsync("api/moneyinmachine");

            balanceDatabase = await balance.Content.ReadAsStringAsync();

            if (response.Content.ReadAsStringAsync().Result.Contains("400"))
            {
                endlessСycle = false;
            }
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert    
        responseContent.Should().Contain("Cannot provide change");
    }

    /// <summary>
    /// We simulate multiple purchases of coffee, enter random numbers and a random coffee name
    /// </summary>
    /// <returns> Cannot provide change </returns>
    [Test]
    public async Task OrderCoffeeWithBogusTest_ReturnsOrderCoffeeDto_ManyOrdersCheckDb()
    {
        // Arrange
        var response = new HttpResponseMessage();
        var faker = new Faker();
        var endlessCycle = true;
        var iteration = 0;

        var client = GetClient();
        
        //Реализую заполнение БД через Bogus
        var inputMoneyInDbJson = JsonContent.Create(TestDataBogus.GenerateInputMoneyInDb(faker));
        await client.PutAsync("api/inputing", inputMoneyInDbJson);

        // Act
        while (endlessCycle)
        {
            iteration++;
            if (iteration == 200)
                endlessCycle = false;

            //Реализую получение случайного названия и случайной внесенной суммы
            var coffeeName = TestDataBogus.GenerateCoffeeName();
            var inputMoney = JsonContent.Create(TestDataBogus.GenerateInputMoney());

            response = await client.PostAsync($"api/order/{coffeeName}", inputMoney);
            var balance = await client.GetAsync("api/moneyinmachine");


            if (response.Content.ReadAsStringAsync().Result.Contains("400"))
            {
                endlessCycle = false;
            }
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert    
        responseContent.Should().Contain("Cannot provide change");
    }

    /// <summary>
    /// Simulating depositing funds into a coffee machine
    /// </summary>
    /// <returns> StatusCode Ok </returns>
    [Test]
    public async Task InputMoneyAsyncTest_ReturnsString_WhenStatusCodeOk()
    {
        // Arrange
        const string expected = "Money deposited";
        var inputMoney = new List<MoneyDto>
        {
            new() { Nominal = (uint)InputBuyerBanknotesEnums.TwoThousand, Count = 10 },
            new() { Nominal = (uint)InputBuyerBanknotesEnums.OneThousand, Count = 10 },
            new() { Nominal = (uint)InputBuyerBanknotesEnums.FiveHundred, Count = 10 }
        };
        var content = JsonContent.Create(inputMoney);

        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.PutAsync("api/inputing", content);

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().Contain(expected);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// We simulate depositing funds into a coffee machine. Banknotes are incorrect
    /// </summary>
    /// <returns> StatusCode BadRequest </returns>
    [Test]
    public async Task InputMoneyAsyncTest_ReturnsString_WhenStatusCodeBadRequest()
    {
        // Arrange
        var inputMoney = new List<MoneyDto>
        {
            new() { Nominal = 400, Count = 5 }
        };
        var content = JsonContent.Create(inputMoney);

        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.PutAsync("api/inputing", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Checking the functionality of obtaining coffee balance
    /// </summary>
    /// <returns> StatusCode OK</returns>
    [Test]
    public async Task GetCoffeeBalanceAsync_ReturnsBalanceCoffeeDto_WhenStatusCodeOk()
    {
        // Arrange
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.GetAsync("api/coffeebalance");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Checking the functionality of obtaining the balance of funds
    /// </summary>
    /// <returns> StatusCode Ok </returns>
    [Test]
    public async Task GetMoneyInMachineAsync_ReturnsMoneyDto_WhenStatusCodeOk()
    {
        // Arrange
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.GetAsync("api/moneyinmachine/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}

