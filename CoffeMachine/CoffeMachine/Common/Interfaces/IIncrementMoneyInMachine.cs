﻿using CoffeeMachine.Dto;

namespace CoffeeMachine.Common.Interfaces;

/// <summary>
/// An interface in which we define a method for depositing money into a coffee machine
/// </summary>
public interface IIncrementMoneyInMachine
{
    /// <summary>
    /// Method for the administrator to deposit money into the coffee machine
    /// </summary>
    /// <param name="inputMoney"> Money deposited into the machine </param>
    /// <exception cref="Exception"> Error associated with incorrectly entered amount into the machine </exception>
    Task<string> IncrementMoneyAsync(List<MoneyDto> inputMoney);
}