namespace CoffeeMachine.Services.Interfaces;

using CoffeeMachine.Dto;

public interface ICoffeeMachineStatusServices
{
    /// <summary>
    /// Getting the coffee balance from the machine
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Task<List<BalanceCoffeeDto>> GetBalanceCoffeeAsync();
    Task<List<BalanceMoneyDto>> GetBalanceMoneyAsync();
}