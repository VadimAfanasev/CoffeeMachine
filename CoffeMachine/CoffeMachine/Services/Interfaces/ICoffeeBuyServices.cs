using CoffeeMachine.Dto;

namespace CoffeeMachine.Services.Interfaces;

/// <summary>
/// An interface that describes methods for buying coffee
/// </summary>
public interface ICoffeeBuyServices
{
    /// <summary>
    /// The method by which coffee is purchased
    /// </summary>
    /// <param name="coffeeType"> Type of coffee </param>
    /// <param name="moneys"> Input moneys </param>
    /// <returns> OrderCoffeeDto </returns>
    /// <response code="200"> Success </response>
    /// <response code="400"> Incorrect data entered </response>
    /// <response code="404"> Entity not found in the system </response>
    Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys);
}

// yt pf,snm elekbnm
