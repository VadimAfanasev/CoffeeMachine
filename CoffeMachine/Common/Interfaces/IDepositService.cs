namespace CoffeeMachine.Common.Interfaces;

/// <summary>
/// Interface that describes methods for working with money in the machine
/// </summary>
public interface IDepositService
{
    /// <summary>
    /// Method in which money is deposited into the coffee machine
    /// </summary>
    /// <param name="inputMoney">Money that will be deposited into the machine</param>
    Task IncrementAvailableNotesAsync(uint[] inputMoney);

    /// <summary>
    /// A method in which we add the amount of purchased coffee to the database
    /// </summary>
    /// <param name="coffeeType"> Type of coffee </param>
    /// <param name="coffeePrice"> Price of coffee</param>
    /// <exception cref="Exception"> No coffee with this name in the system </exception>
    Task IncrementCoffeeBalanceAsync(string coffeeType, uint coffeePrice);
}