namespace CoffeeMachine.Services.Interfaces;

using CoffeeMachine.Dto;

public interface IInputMoneyServices
{
    Task InputingAsync(List<InputMoneyDto> inputMoney);
}