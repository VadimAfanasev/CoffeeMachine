using Bogus;
using CoffeeMachine.Common.Constants;
using CoffeeMachine.Common.Enums;

namespace CoffeeMachine.Tests.Infrastructure;

internal class TestDataBogus
{
    public static List<MoneyDto> GenerateInputMoneyInDb(Faker faker)
    {
        var inputMoneyList = new List<MoneyDto>();

        foreach (InputAdminBanknotesEnums nominal in Enum.GetValues(typeof(InputAdminBanknotesEnums)))
        {
            uint count = (uint)faker.Random.Number(0, 100);
            var moneyDto = new MoneyDto { Count = count, Nominal = (uint)nominal };
            inputMoneyList.Add(moneyDto);
        }

        return inputMoneyList;
    }

    public static string GenerateCoffeeName()
    {
        var faker = new Faker();
        
        var coffeeNames = new[] { CoffeeNames.CAPPUCCINO, CoffeeNames.LATTE, CoffeeNames.AMERICANO };
        return faker.Random.ArrayElement(coffeeNames);
    }

    public static uint[] GenerateInputMoney()
    {
        var faker = new Faker();
        var inputMoneys = new[]
        {
            new[] { (uint)InputBuyerBanknotesEnums.OneThousand },
            new[] { (uint)InputBuyerBanknotesEnums.TwoThousand },
            new[] { (uint)InputBuyerBanknotesEnums.FiveThousand }
        };
        
        return faker.Random.ArrayElement(inputMoneys);
    }
}

