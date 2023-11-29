using CoffeeMachine.Common.Enums;

namespace CoffeeMachine.Tests.Infrastructure;

public class RandomCombinationGenerator
{
    private static Random _random = new Random(Seed:5);


    public uint[] GenerateRandomCombination()
    {
        List<uint> numbers = new List<uint> { (uint)InputBuyerBanknotesEnums.OneThousand, 
            (uint)InputBuyerBanknotesEnums.TwoThousand, (uint)InputBuyerBanknotesEnums.FiveThousand };

        int count = _random.Next(1, 3);
        List<uint> combination = new List<uint>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = _random.Next(0, numbers.Count);
            combination.Add(numbers[randomIndex]);
        }

        return combination.ToArray();

    }
}