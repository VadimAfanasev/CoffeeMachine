using CoffeeMachine.Common.Constants;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

using LazyCache;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Services;

/// <summary>
/// The class in which we get information about the coffee machine
/// </summary>
public class CoffeeMachineStatusServices : ICoffeeMachineStatusServices
{
    /// <summary>
    /// Input lazy cache dependency injection
    /// </summary>
    private readonly IAppCache _cache;

    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Constructor of a class in which we get information about the coffee machine
    /// </summary>
    /// <param name="db"> Context database </param>
    /// <param name="cache"> Lazy cache </param>
    public CoffeeMachineStatusServices(CoffeeContext db, IAppCache cache)
    {
        _db = db;
        _cache = cache;
    }

    /// <inheritdoc />
    public async Task<List<BalanceCoffeeDto>> GetBalanceCoffeeAsync()
    {
        var balanceCoffee = await _cache.GetOrAddAsync(
            CacheKeys.COFFEE_BUY,
            async () =>
            {
                var coffeeBalances = await _db.CoffeesDb.ToListAsync();

                var balanceCoffee = coffeeBalances.Select(c => new BalanceCoffeeDto
                {
                    Name = c.Name,
                    Balance = c.Balance
                }).ToList();

                var totalBalance = coffeeBalances.Sum(c => c.Balance);

                var totalBalanceDto = new BalanceCoffeeDto
                {
                    Name = "Total",
                    Balance = (uint)totalBalance
                };

                balanceCoffee.Add(totalBalanceDto);

                return balanceCoffee;
            },
            TimeSpan.FromMinutes(10)); // Время жизни кэша

        return balanceCoffee ?? throw new Exception("Entity not found in the system");
    }

    /// <inheritdoc />
    public async Task<List<MoneyDto>> GetBalanceMoneyAsync()
    {
        var balanceMoney = await _cache.GetOrAddAsync(
            CacheKeys.INPUT_MONEY,
            async () =>
            {
                var moneyBalances = await _db.MoneyInMachinesDb.ToListAsync();

                var balanceMoney = moneyBalances.Select(c => new MoneyDto
                {
                    Nominal = c.Nominal,
                    Count = c.Count
                }).ToList();

                return balanceMoney.OrderByDescending(n => n.Nominal).ToList();
            },
            TimeSpan.FromMinutes(10)); // Время жизни кэша

        return balanceMoney ?? throw new Exception("Entity not found in the system");
    }
}