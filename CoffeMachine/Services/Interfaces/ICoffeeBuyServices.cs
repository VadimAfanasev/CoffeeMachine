namespace CoffeeMachine.Services.Interfaces;

using CoffeeMachine.Dto;

public interface ICoffeeBuyServices
{
    Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys);
    uint SumUintArray(uint[] moneys);
}