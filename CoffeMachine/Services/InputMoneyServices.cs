using CoffeeMachine.Common;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;
using LazyCache;

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
    /// Input lazy cache dependency injection
    /// </summary>
    private readonly IAppCache _cache;

    /// <summary>
    /// Constructor of the class in which we deposit money into the coffee machine
    /// </summary>
    /// <param name="db"> Context database </param>
    /// <param name="incrementMoneyInMachine"> Methods for incrementing money in machine </param>
    public InputMoneyServices(CoffeeContext db, IIncrementMoneyInMachine incrementMoneyInMachine, IAppCache cache)
    {
        _db = db;
        _incrementMoneyInMachine = incrementMoneyInMachine;
        _cache = cache;
    }

    /// <inheritdoc />
    public async Task<string> InputingAsync(List<MoneyDto> inputMoney)
    {
        var moneyList = inputMoney.Select(a => a.Nominal).Distinct().ToArray();
        if (!_db.MoneyInMachinesDb.Any(m => moneyList.Contains(m.Nominal)))
            throw new ArgumentException("Invalid banknotes type");

        var result = await _incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);

        var cacheKey = CacheKeys.inputMoney;
        _cache.Remove(cacheKey);

        return result;
    }
}