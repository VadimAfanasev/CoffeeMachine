namespace CoffeeMachine.Services.Interfaces;

using CoffeeMachine.Dto;

/// <summary>
/// An interface that describes methods for buying coffee
/// </summary>
public interface ICoffeeBuyServices
{
    /// <summary>
    /// The method by which coffee is purchased
    /// </summary>
    /// <param name="coffeeType"></param>
    /// <param name="moneys"></param>
    /// <returns>OrderCoffeeDto</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Incorrect data entered</response>
    /// <response code="404">Entity not found in the system</response>
    Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys);

    /// <summary>
    /// Method for getting the amount of an array of entered money
    /// </summary>
    /// <param name="moneys"></param>
    /// <returns>uint</returns>
    uint SumUintArray(uint[] moneys);
}