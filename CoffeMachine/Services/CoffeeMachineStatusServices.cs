namespace CoffeeMachine.Services;

using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

public class CoffeeMachineStatusServices : ICoffeeMachineStatusServices
{
    private readonly CoffeeContext _db;

    public CoffeeMachineStatusServices(CoffeeContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<List<BalanceCoffeeDto>> GetBalanceCoffeeAsync()
    {
        var balanceCoffee = new List<BalanceCoffeeDto>();
        var coffeeBalances = await _db.Coffees.ToListAsync();

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

        if (balanceCoffee == null)
            throw new Exception("Entity not found in the system");

        return balanceCoffee;
    }

    /// <summary>
    /// We get a list of funds available in the machine
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<BalanceMoneyDto>> GetBalanceMoneyAsync()
    {
        var balanceMoney = new List<BalanceMoneyDto>();
        var moneyBalances = await _db.MoneyInMachines.ToListAsync();

        foreach (var balance in moneyBalances)
        {
            var balanceDto = new BalanceMoneyDto
            {
                Nominal = balance.Nominal,
                Count = balance.Count
            };
            balanceMoney.Add(balanceDto);
        }

        var sortedBalanceMoney = balanceMoney.OrderByDescending(n => n.Nominal).ToList();

        if (sortedBalanceMoney == null)
            throw new Exception("Entity not found in the system");

        return sortedBalanceMoney;
    }
}