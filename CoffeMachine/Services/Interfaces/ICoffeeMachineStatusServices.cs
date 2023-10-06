namespace CoffeMachine.Services.Interfaces
{
    using CoffeMachine.Dto;

    public interface ICoffeeMachineStatusServices
    {
        List<BalanceCoffeeDto> GetBalanceCoffee();
        List<BalanceMoneyDto> GetBalanceMoney();
    }
}