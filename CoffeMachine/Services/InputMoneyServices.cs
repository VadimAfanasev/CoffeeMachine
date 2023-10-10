namespace CoffeeMachine.Services;

using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;

/// <summary>
/// The class that implements depositing funds into a coffee machine
/// </summary>
public class InputMoneyServices : IInputMoneyServices
{
    /// <summary>
    /// Array of banknotes available for depositing
    /// </summary>
    private readonly uint[] _banknotes = { 5000, 2000, 1000, 500, 200, 100, 50 };
    /// <summary>
    /// Injecting money depositing methods
    /// </summary>
    private readonly IIncrementMoneyInMachine _incrementMoneyInMachine;

    /// <summary>
    /// Constructor of the class in which we deposit money into the coffee machine
    /// </summary>
    public InputMoneyServices(IIncrementMoneyInMachine incrementMoneyInMachine)
    {
        _incrementMoneyInMachine = incrementMoneyInMachine;
    }

    /// <inheritdoc />
    public async Task InputingAsync(List<InputMoneyDto> inputMoney)
    {
        if (!inputMoney.All(c => _banknotes.Contains(c.Nominal)))
            throw new ArgumentException("Invalid banknotes type");

        await _incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);
    }
}