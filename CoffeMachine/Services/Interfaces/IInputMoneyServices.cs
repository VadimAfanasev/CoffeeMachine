namespace CoffeeMachine.Services.Interfaces;

using CoffeeMachine.Dto;

/// <summary>
/// Interface describing depositing funds into a coffee machine
/// </summary>
public interface IInputMoneyServices
{

    /// <summary>
    /// Depositing funds into a coffee machine
    /// </summary>
    /// <param name="inputMoney"></param>
    /// <response code="400">Incorrect data entered</response>
    Task InputingAsync(List<InputMoneyDto> inputMoney);
}