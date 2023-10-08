using CoffeeMachine.Dto;

namespace CoffeeMachine.Services.Interfaces
{
    public interface IInputMoneyServices
    {
        Task InputingAsync(List<InputMoneyDto> inputMoney);
    }
}