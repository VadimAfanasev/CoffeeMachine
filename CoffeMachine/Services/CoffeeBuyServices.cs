using static CoffeeMachine.Common.EnumBanknotes;

namespace CoffeeMachine.Services;

using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

public class CoffeeBuyServices : ICoffeeBuyServices
{
    private readonly ICalculateChange _calculateChange;
    private readonly CoffeeContext _db;
    private readonly IDecrementAvailableNotes _decrementAvailableNote;
    private readonly IIncrementAvailableNotes _incrementAvailableNote;
    private readonly IIncrementCoffeeBalances _incrementCoffeeBalances;

    public CoffeeBuyServices(CoffeeContext db, ICalculateChange calculateChange,
        IDecrementAvailableNotes decrementAvailableNotes,
        IIncrementAvailableNotes incrementAvailableNotes, IIncrementCoffeeBalances incrementCoffeeBalances)
    {
        _db = db;
        _calculateChange = calculateChange;
        _decrementAvailableNote = decrementAvailableNotes;
        _incrementAvailableNote = incrementAvailableNotes;
        _incrementCoffeeBalances = incrementCoffeeBalances;
    }

    /// <summary>
    /// The method by which coffee is purchased
    /// </summary>
    /// <param name="coffeeType"></param>
    /// <param name="moneys"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public async Task<OrderCoffeeDto> BuyingCoffeeAsync(string coffeeType, uint[] moneys)
    {
        if (!_db.Coffees.Any(c => c.Name == coffeeType))
            throw new InvalidDataException("Invalid coffee type");

        if (!moneys.All(c => Enum.IsDefined(typeof(Banknotes), c)))
            throw new InvalidDataException("Invalid banknotes type");

        var moneysUint = SumUintArray(moneys);
        var coffeePrice = GetCoffeePrice(coffeeType);

        if (moneysUint < coffeePrice)
            throw new ArgumentException("The amount deposited is less than required");

        var changeAmount = moneysUint - coffeePrice;

        var change = await _calculateChange.CalculateAsync(changeAmount);

        if (change == null)
            throw new ArgumentException("Cannot provide change");

        await _incrementAvailableNote.IncrementAvailableNoteAsync(moneys);

        await _decrementAvailableNote.DecrementAvailableNoteAsync(change);

        await _incrementCoffeeBalances.IncrementCoffeeBalanceAsync(coffeeType, coffeePrice);

        var changeDto = ChangeToDto(change);

        return changeDto;
    }

    /// <summary>
    /// We calculate the amount of deposited funds to calculate the change
    /// </summary>
    /// <param name="moneys"></param>
    /// <returns></returns>
    public uint SumUintArray(uint[] moneys)
    {
        var sum = moneys.Sum(n => n);
        return (uint)sum;
    }

    /// <summary>
    /// Transfer the user's change to the Dto
    /// </summary>
    /// <param name="change"></param>
    /// <returns></returns>
    private static OrderCoffeeDto ChangeToDto(List<uint> change)
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
    /// <param name="coffeeType"></param>
    /// <returns></returns>
    private uint GetCoffeePrice(string coffeeType)
    {
        var coffeePrice = _db.Coffees
            .Where(c => c.Name == coffeeType)
            .Select(x => x.Price).ToList();

        return coffeePrice[0];
    }
}