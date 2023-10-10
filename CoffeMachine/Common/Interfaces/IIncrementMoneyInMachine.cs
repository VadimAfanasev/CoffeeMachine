using CoffeeMachine.Dto;

namespace CoffeeMachine.Common.Interfaces;

public interface IIncrementMoneyInMachine
{
    Task IncrementMoneyAsync(List<InputMoneyDto> inputMoney);
}