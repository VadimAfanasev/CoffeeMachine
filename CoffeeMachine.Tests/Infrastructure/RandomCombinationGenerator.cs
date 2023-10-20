namespace CoffeeMachine.Tests.Infrastructure
{
    public class RandomCombinationGenerator
    {
        private static Random random = new Random(Seed:5);


        public uint[] GenerateRandomCombination()
        {
            List<uint> numbers = new List<uint> { 500, 1000, 2000, 5000 };
            List<uint> combination = new List<uint>();

            int count = random.Next(1, numbers.Count + 1);
            for (int i = 0; i < count; i++)
            {
                int randomIndex = random.Next(0, numbers.Count);
                combination.Add(numbers[randomIndex]);
            }

            if (combination.Sum(c=>c) > 500 )
            {
                return combination.ToArray();
            }
            else return new uint[] { 1000 };

        }
    }
}
