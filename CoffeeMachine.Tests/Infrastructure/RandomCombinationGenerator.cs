using CoffeeMachine.Common.Enums;

namespace CoffeeMachine.Tests.Infrastructure;

public class RandomCombinationGenerator
{
    private static Random random = new Random(Seed:5);


    public uint[] GenerateRandomCombination()
    {
        List<uint> numbers = new List<uint> { (uint)InputBuyerBanknotesEnums.OneThousand, 
            (uint)InputBuyerBanknotesEnums.TwoThousand, (uint)InputBuyerBanknotesEnums.FiveThousand };

        int count = random.Next(1, 3);
        List<uint> combination = new List<uint>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(0, numbers.Count);
            combination.Add(numbers[randomIndex]);
        }

        return combination.ToArray();

    }
}