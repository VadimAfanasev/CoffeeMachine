namespace CoffeeMachine.Common.Interfaces
{
    public interface IIncrementCoffeeBalances
    {
        Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice);
    }
}