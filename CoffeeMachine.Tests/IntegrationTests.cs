﻿using Azure;
using CoffeeMachine.Tests.Infrastructure;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CoffeeMachine.Tests;

[TestFixture]
public class IntegrationTests : CustomBaseTest
{
    [Test]
    public async Task OrderCoffeeTest_ReturnsOrderCoffeeDto_WhenDtoAreEqual()
    {
        // Arrange
        var inputMoney = new uint[] { 2000, 500 };
        const string coffeeName = "Cappuccino";
        var expected = new OrderCoffeeDto() { Change = new List<uint> { 1000, 500, 200, 200 } };

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

    [Test]
    public async Task OrderCoffeeTest_ReturnsException_InvalidCoffeeName()
    {
        // Arrange
        var inputMoney = new uint[] { 2000, 500 };
        const string invalidCoffeeName = "InvalidCoffeeName";

        var client = GetClient();
        var content = JsonContent.Create(inputMoney);

        // Act
        var response = await client.PostAsync($"api/order/{invalidCoffeeName}", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().Contain("Invalid coffee type");
    }

    [Test]
    public async Task OrderCoffeeTest_ReturnsOrderCoffeeDto_InvalidMoneyAmount()
    {
        // Arrange
        var inputMoney = new uint[] { 500 };
        const string coffeeName = "Cappuccino";

        var client = GetClient();
        var content = JsonContent.Create(inputMoney);

        // Act
        var response = await client.PostAsync($"api/order/{coffeeName}", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().Contain("The amount deposited is less than required");
    }

    [Test]
    public async Task OrderCoffeeTest_ReturnsOrderCoffeeDto_ManyOrdersCheckDb()
    {
        // Arrange
        const string coffeeName = "Cappuccino";
        var response = new HttpResponseMessage();
        string balanceDatabase = "";

        var endlessСycle = true;
        int iteration = 0;

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
            uint[] combination = generator.GenerateRandomCombination();
            var content = JsonContent.Create(combination);
            var contentNew = new uint[] { 5000 };
            var contentNumber = JsonContent.Create(contentNew);

            iteration++;
            if (iteration == 100)
                endlessСycle = false;
            response = await client.PostAsync($"api/order/{coffeeName}", contentNumber);
            var balance = await client.GetAsync("api/moneyinmachine");

            balanceDatabase = await balance.Content.ReadAsStringAsync();

            if (response.Content.ReadAsStringAsync().Result.Contains("400"))
            {
                endlessСycle = false;
            }
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var balanceDatabaseNew = balanceDatabase;
        // Assert    
        responseContent.Should().Contain("Cannot provide change");
    }

    [Test]
    public async Task LoginTest_ReturnsString_WhenStatusCodeOk()
    {
        // Arrange
        var user = new UserModel
        {
            UserName = "Admin",
            Password = "Admin"
        };
        var content = JsonContent.Create(user);

        var client = GetClient();

        // Act
        var response = await client.PostAsync($"api/login", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().NotContain("User not found");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task LoginTest_ReturnsString_WhenStatusCodeNotFound()
    {
        // Arrange
        var user = new UserModel
        {
            UserName = "AnyUser",
            Password = "AnyUser"
        };
        var content = JsonContent.Create(user);

        var client = GetClient();

        // Act
        var response = await client.PostAsync($"api/login", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().Contain("User not found");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task InputMoneyAsyncTest_ReturnsString_WhenStatusCodeOk()
    {
        // Arrange
        const string expected = "Money deposited";
        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = 2000, Count = 10 },
            new MoneyDto() { Nominal = 1000, Count = 10 },
            new MoneyDto() { Nominal = 500, Count = 10 }
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

    [Test]
    public async Task InputMoneyAsyncTest_ReturnsString_WhenStatusCodeBadRequest()
    {
        // Arrange
        var inputMoney = new List<MoneyDto>
        {
            new MoneyDto() { Nominal = 400, Count = 5 }
        };
        var content = JsonContent.Create(inputMoney);

        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.PutAsync("api/inputing", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetCoffeeBalanceAsync_ReturnsBalanceCoffeeDto_WhenStatusCodeOk()
    {
        // Arrange
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.GetAsync("api/coffeebalance");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetMoneyInMachineAsync_ReturnsMoneyDto_WhenStatusCodeOk()
    {
        // Arrange
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.GetAsync("api/moneyinmachine/");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}
