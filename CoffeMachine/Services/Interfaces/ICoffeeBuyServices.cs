using CoffeeMachine.Dto;

namespace CoffeeMachine.Services.Interfaces
{
    public interface ICoffeeBuyServices
    {
        Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys);
        uint SumUintArray(uint[] moneys);
    }
}