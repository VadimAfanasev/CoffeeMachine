using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

namespace CoffeeMachine.Services;

/// <summary>
/// The class that implements depositing funds into a coffee machine
/// </summary>
public class InputMoneyServices : IInputMoneyServices
{
    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Injecting money depositing methods
    /// </summary>
    private readonly IIncrementMoneyInMachine _incrementMoneyInMachine;

    /// <summary>
    /// Constructor of the class in which we deposit money into the coffee machine
    /// </summary>
    public InputMoneyServices(CoffeeContext db, IIncrementMoneyInMachine incrementMoneyInMachine)
    {
        _db = db;
        _incrementMoneyInMachine = incrementMoneyInMachine;
    }

    /// <inheritdoc />
    public async Task<string> InputingAsync(List<MoneyDto> inputMoney)
    {
        //if (!inputMoney.All(c => _banknotes.Contains(c.Nominal)))
        //    throw new ArgumentException("Invalid banknotes type");

        foreach (var money in inputMoney)
        {
            if (!_db.MoneyInMachines.Any(m => m.Nominal == money.Nominal))
                throw new ArgumentException("Invalid banknotes type");
        }

        var result = await _incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);

        return result;
    }
}