namespace CoffeeMachine.Common.Interfaces;

/// <summary>
/// An interface in which we describe adding the amount of purchased coffee to the database
/// </summary>
public interface IIncrementCoffeeBalances
{
    /// <summary>
    /// A method in which we add the amount of purchased coffee to the database
    /// </summary>
    /// <param name="coffeeType"> </param>
    /// <param name="coffeePrice"> </param>
    /// <exception cref="Exception"> </exception>
    Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice);
}