namespace CoffeeMachine.Services;

using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;

public class InputMoneyServices : IInputMoneyServices
{
    private readonly uint[] _banknotes = { 5000, 2000, 1000, 500, 200, 100, 50 };
    private readonly IIncrementMoneyInMachine _incrementMoneyInMachine;

    public InputMoneyServices(IIncrementMoneyInMachine incrementMoneyInMachine)
    {
        _incrementMoneyInMachine = incrementMoneyInMachine;
    }

    /// <summary>
    /// Depositing money into the machine yourself
    /// </summary>
    /// <param name="inputMoney"></param>
    /// <exception cref="ArgumentException"></exception>
    public async Task InputingAsync(List<InputMoneyDto> inputMoney)
    {
        if (!inputMoney.All(c => _banknotes.Contains(c.Nominal)))
            throw new ArgumentException("Invalid banknotes type");

        await _incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);
    }
}