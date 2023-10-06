namespace CoffeMachine.Services.Interfaces
{
    using CoffeMachine.Dto;

    public interface ICoffeeBuyServices
    {
        OrderCoffeeDto BuyingCoffee(string coffeeType, uint[] moneys);
        uint SumUintArray(uint[] moneys);
    }
}