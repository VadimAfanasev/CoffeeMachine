using CoffeeMachine.Dto;

using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Services.Interfaces;

/// <summary>
/// Interface describing depositing funds into a coffee machine
/// </summary>
public interface IInputMoneyServices
{
    /// <summary>
    /// Depositing funds into a coffee machine
    /// </summary>
    /// <param name="inputMoney"> </param>
    /// <response code="400"> Incorrect data entered </response>
    Task<string> InputingAsync(List<MoneyDto> inputMoney);
}