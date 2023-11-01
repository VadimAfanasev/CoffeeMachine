using CoffeeMachine.Common;
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
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Input lazy cache dependency injection
    /// </summary>
    private readonly IAppCache _cache;

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
        var cacheKey = CacheKeys.coffeeBuy;

        var balanceCoffee = await _cache.GetOrAddAsync(
            cacheKey,
            async () =>
            {
                var balanceCoffee = new List<BalanceCoffeeDto>();

                var coffeeBalances = await _db.CoffeesDb.ToListAsync();

                uint totalBalance = 0;

                foreach (var balance in coffeeBalances)
                {
                    var balanceDto = new BalanceCoffeeDto
                    {
                        Name = balance.Name,
                        Balance = balance.Balance
                    };
                    balanceCoffee.Add(balanceDto);
                    totalBalance += balance.Balance;
                }

                var totalBalanceDto = new BalanceCoffeeDto
                {
                    Name = "Total",
                    Balance = totalBalance
                };

                balanceCoffee.Add(totalBalanceDto);

                return balanceCoffee;
            },
            TimeSpan.FromMinutes(10)); // Время жизни кэша

        if (balanceCoffee == null)
            throw new Exception("Entity not found in the system");

        return balanceCoffee;
    }

    /// <inheritdoc />
    public async Task<List<MoneyDto>> GetBalanceMoneyAsync()
    {
        var cacheKey = CacheKeys.inputMoney;

        var balanceMoney = await _cache.GetOrAddAsync(
            cacheKey,
            async () =>
            {
                var balanceMoney = new List<MoneyDto>();
                var moneyBalances = await _db.MoneyInMachinesDb.ToListAsync();

                foreach (var balance in moneyBalances)
                {
                    var balanceDto = new MoneyDto
                    {
                        Nominal = balance.Nominal,
                        Count = balance.Count
                    };
                    balanceMoney.Add(balanceDto);
                }

                var sortedBalanceMoney = balanceMoney.OrderByDescending(n => n.Nominal).ToList();

                return sortedBalanceMoney;
            },
            TimeSpan.FromMinutes(10)); // Время жизни кэша

        if (balanceMoney == null)
            throw new Exception("Entity not found in the system");

        return balanceMoney;
    }
}