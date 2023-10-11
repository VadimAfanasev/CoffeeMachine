namespace CoffeeMachine.Common.Interfaces;

/// <summary>
/// An interface in which we implement a method for adding user-deposited bills
/// </summary>
public interface IIncrementAvailableNotes
{
    /// <summary>
    /// Adding money contributed by the user to the table
    /// </summary>
    /// <param name="inputMoney"> </param>
    /// <exception cref="Exception"> </exception>
    Task IncrementAvailableNoteAsync(uint[] inputMoney);
}