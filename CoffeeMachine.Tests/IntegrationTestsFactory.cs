using CoffeeMachine.Tests.Infrastructure;
using Moq;
using Newtonsoft.Json;
using System.Text;

namespace CoffeeMachine.Tests;

[TestFixture]
public class TemperatureTests : CustomBaseTest
{
    //[Test]
    //public async Task OrderCoffeeTest()
    //{
    //    var intputMoney = new uint[] { 2000, 500 };
    //    var coffeeName = "Cappuccino";
    //    var expected = new OrderCoffeeDto();
    //    ExternalServicesMock.CoffeeMachineClient
    //        .Setup(x => x.PlaceOrder(coffeeName, intputMoney))
    //        .ReturnsAsync(expected);

    //    var client = GetClient();

    //    var response = await client.GetAsync("api/order/");
    //    var responseContent = await response.Content.ReadAsStringAsync();

    //    //Assert.AreEqual(expected, responseContent);
    //    //ExternalServicesMock.TemperatureApiClient.Verify(x => x.GetTemperatureAsync(), Times.OncMetrics);
    //}
    [Test]
    public async Task OrderCoffeeTest()
    {
        var inputMoney = new uint[] { 2000, 500 };
        var coffeeName = "Cappuccino";
        var expected = new OrderCoffeeDto() { Change = new List<uint> { 1000, 500, 200, 200 } };
        ExternalServicesMock.CoffeeMachineApiClient
            .Setup(x => x.BuyingCoffeeAsync(coffeeName, inputMoney))
            .ReturnsAsync(expected);

        var client = GetClient();

        var response = await client.GetAsync($"api/order/{coffeeName}");
        var responseContent = await response.Content.ReadAsStringAsync();


        //Assert.That(responseContent, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        //ExternalServicesMock.CoffeeMachineApiClient.Verify(x => x.BuyingCoffeeAsync("Cappuccino", intputMoney), Times.Once);
        //Assert.AreEqual(expected, responseContent);
        //ExternalServicesMock.CoffeeMachineApiClient.Verify(x => x.BuyingCoffeeAsync(coffeeName, inputMoney), Times.Once);
    }

    [Test]
    public async Task OrderCoffeeTestNew()
    {
        var intputMoney = new uint[] { 2000, 500 };
        var coffeeName = "Cappuccino";
        var expected = new OrderCoffeeDto();
        ExternalServicesMock.CoffeeMachineApiClient
            .Setup(x => x.BuyingCoffeeAsync(coffeeName, intputMoney))
            .ReturnsAsync(expected);

        var client = GetClient();

        // JSON-представление данных
        var content = new StringContent(JsonConvert.SerializeObject(intputMoney), Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"api/order/{coffeeName}", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        //Assert.AreEqual(expected, responseContent);
        //ExternalServicesMock.CoffeeMachineApiClient.Verify(x => x.BuyingCoffeeAsync(coffeeName, intputMoney), Times.Once);
    }
}