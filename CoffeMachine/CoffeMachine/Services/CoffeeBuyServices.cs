using CoffeeMachine.Common;
using CoffeeMachine.Common.Enums;
using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

using LazyCache;

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
    /// Input lazy cache dependency injection
    /// </summary>
    private readonly IAppCache _cache;

    /// <summary>
    /// Constructor of the class in which coffee is purchased
    /// </summary>
    public CoffeeBuyServices(CoffeeContext db, IDepositService depositService, IChangeCalculation changeCalculation, IAppCache cache)
    {
        _db = db;
        _depositService = depositService;
        _changeCalculation = changeCalculation;
        _cache = cache;
    }

    /// <inheritdoc />
    public async Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys)
    {
        if (!await _db.CoffeesDb.AnyAsync(c => c.Name == coffeeType))
            throw new InvalidDataException("Invalid coffee type");

        if (!moneys.All(c => Enum.IsDefined(typeof(InputBuyerBanknotesEnums), c)))
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

        var cacheKey = CacheKeys.coffeeBuy;
        _cache.Remove(cacheKey);

        return changeDto;
    }

    /// <summary>
    /// Transfer the user's change to the Dto
    /// </summary>
    /// <param name="change"> </param>
    /// <returns> OrderCoffeeDto </returns>
    private OrderCoffeeDto ChangeToDto(List<uint> change)
    {
        var changeDto = new OrderCoffeeDto
        {
            Change = change
        };

        return changeDto;
    }

    /// <summary>
    /// Calculating the cost of coffee
    /// </summary>
    /// <param name="coffeeType"> </param>
    /// <returns> uint </returns>
    private uint GetCoffeePrice(string coffeeType)
    {
        var coffeePrice = _db.CoffeesDb
            .Where(c => c.Name == coffeeType)
            .Select(x => x.Price).FirstOrDefault();

        return coffeePrice;
    }



    /// <summary>
    /// Method for getting the amount of an array of entered money
    /// </summary>
    /// <param name="moneys"> </param>
    /// <returns> uint </returns>
    private uint SumUintArray(uint[] moneys)
    {
        var sum = moneys.Sum(n => n);
        return (uint)sum;
    }
}