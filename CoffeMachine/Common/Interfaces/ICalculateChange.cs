namespace CoffeeMachine.Common.Interfaces;

/// <summary>
/// An interface that describes the method for calculating change
/// </summary>
public interface ICalculateChange
{
    /// <summary>
    /// Method for calculating change
    /// </summary>
    /// <param name="amount"> </param>
    /// <returns> The List of <paramref name="amount" /> </returns>
    Task<List<uint>> CalculateAsync(uint amount);
}