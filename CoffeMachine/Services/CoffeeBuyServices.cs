using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services.Interfaces;

using static CoffeeMachine.Common.EnumBanknotes;

namespace CoffeeMachine.Services;

/// <summary>
/// The class in which the purchase of coffee is implemented
/// </summary>
public class CoffeeBuyServices : ICoffeeBuyServices
{
    /// <summary>
    /// Injecting of change calculation methods
    /// </summary>
    private readonly ICalculateChange _calculateChange;

    /// <summary>
    /// Injecting the database context CoffeeContext
    /// </summary>
    private readonly CoffeeContext _db;

    /// <summary>
    /// Implementation of methods for deducting change from the database
    /// </summary>
    private readonly IDecrementAvailableNotes _decrementAvailableNote;

    /// <summary>
    /// Implementation of methods for entering change into the database
    /// </summary>
    private readonly IIncrementAvailableNotes _incrementAvailableNote;

    /// <summary>
    /// Implementation of methods for entering purchased coffee into the database
    /// </summary>
    private readonly IIncrementCoffeeBalances _incrementCoffeeBalances;

    /// <summary>
    /// Constructor of the class in which coffee is purchased
    /// </summary>
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public uint SumUintArray(uint[] moneys)
    {
        var sum = moneys.Sum(n => n);
        return (uint)sum;
    }

    /// <inheritdoc />
    public OrderCoffeeDto ChangeToDto(List<uint> change)
    {
        var changeDto = new OrderCoffeeDto
        {
            Change = change
        };

        return changeDto;
    }

    /// <inheritdoc />
    public uint GetCoffeePrice(string coffeeType)
    {
        var coffeePrice = _db.Coffees
            .Where(c => c.Name == coffeeType)
            .Select(x => x.Price).ToList();

        return coffeePrice[0];
    }
}