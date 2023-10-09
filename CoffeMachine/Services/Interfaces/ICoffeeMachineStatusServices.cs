namespace CoffeeMachine.Services.Interfaces
{
    using CoffeeMachine.Dto;

    public interface ICoffeeMachineStatusServices
    {
        Task<List<BalanceCoffeeDto>> GetBalanceCoffeeAsync();
        Task<List<BalanceMoneyDto>> GetBalanceMoneyAsync();
    }
}