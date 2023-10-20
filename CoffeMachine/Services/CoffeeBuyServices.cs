using CoffeeMachine.Common;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine.Services;

/// <summary>
/// The class in which the purchase of coffee is implemented
/// </summary>
public class CoffeeBuyServices : ICoffeeBuyServices
{
    /// <summary>
    /// Injecting of change calculation methods
    /// </summary>
    private readonly IChangeCalculation _changeCalculation;

    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Injecting of manipulation with money methods
    /// </summary>
    private readonly IDepositService _depositService;

    /// <summary>
    /// Constructor of the class in which coffee is purchased
    /// </summary>
    public CoffeeBuyServices(CoffeeContext db, IDepositService depositService, IChangeCalculation changeCalculation)
    {
        _db = db; 
        _depositService = depositService;
        _changeCalculation = changeCalculation;
    }

    /// <inheritdoc />
    public async Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys)
    {
        if (! (await _db.CoffeesDb.AnyAsync(c => c.Name == coffeeType)))
            throw new InvalidDataException("Invalid coffee type");

        if (!moneys.All(c => Enum.IsDefined(typeof(EnumBanknotes.Banknotes), c)))
            throw new InvalidDataException("Invalid banknotes type");

        var moneysUint = SumUintArray(moneys);
        var coffeePrice = GetCoffeePrice(coffeeType);

        if (moneysUint < coffeePrice)
            throw new ArgumentException("The amount deposited is less than required");

        var changeAmount = moneysUint - coffeePrice;

        await _depositService.IncrementAvailableNotesAsync(moneys);

        var change = await _changeCalculation.CalculateAsync(changeAmount);

        if (change == null)
            throw new ArgumentException("Cannot provide change");

        await _depositService.IncrementCoffeeBalanceAsync(coffeeType, coffeePrice);

        var changeDto = ChangeToDto(change);

        return changeDto;
    }

    /// <inheritdoc />
    private uint SumUintArray(uint[] moneys)
    {
        var sum = moneys.Sum(n => n);
        return (uint)sum;
    }

    /// <inheritdoc />
    private OrderCoffeeDto ChangeToDto(List<uint> change)
    {
        var changeDto = new OrderCoffeeDto
        {
            Change = change
        };

        return changeDto;
    }

    /// <inheritdoc />
    private uint GetCoffeePrice(string coffeeType)
    {
        var coffeePrice = _db.CoffeesDb
            .Where(c => c.Name == coffeeType)
            .Select(x => x.Price).FirstOrDefault();

        return coffeePrice;
    }
}