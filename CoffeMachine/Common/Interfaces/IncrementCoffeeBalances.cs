namespace CoffeMachine.Common.Interfaces
{
    public interface IIncrementCoffeeBalances
    {
        void IncrementCoffeeBalance(string coffeeType, uint coffeePrice);
    }
}
