using CoffeeMachine.Dto;

namespace CoffeeMachine.Services.Interfaces
{
    public interface ICoffeeMachineStatusServices
    {
        Task<List<BalanceCoffeeDto>> GetBalanceCoffeeAsync();
        Task<List<BalanceMoneyDto>> GetBalanceMoneyAsync();
    }
}