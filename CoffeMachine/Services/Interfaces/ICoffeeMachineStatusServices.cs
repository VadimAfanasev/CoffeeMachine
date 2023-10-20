using CoffeeMachine.Dto;

namespace CoffeeMachine.Services.Interfaces;

/// <summary>
/// An interface that describes methods for obtaining information about a coffee machine
/// </summary>
public interface ICoffeeMachineStatusServices
{
    /// <summary>
    /// Getting the coffee balance from the machine
    /// </summary>
    /// <returns> List of BalanceCoffeeDto </returns>
    /// <exception cref="Exception"> Entity not found in the system </exception>
    Task<List<BalanceCoffeeDto>> GetBalanceCoffeeAsync();

    /// <summary>
    /// We get a list of funds available in the machine
    /// </summary>
    /// <returns> List of MoneyDto </returns>
    /// <exception cref="Exception"> Entity not found in the system </exception>
    Task<List<MoneyDto>> GetBalanceMoneyAsync();
}