namespace CoffeMachine.Services.Interfaces
{
    public interface ICoffeeBuyServices
    {
        List<uint> BuyingCoffee(string coffeeType, uint[] moneys);
        uint SumUintArray(uint[] moneys);
    }
}
