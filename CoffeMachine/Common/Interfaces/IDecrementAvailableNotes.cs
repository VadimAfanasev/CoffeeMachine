namespace CoffeeMachine.Common.Interfaces;

/// <summary>
/// An interface that implements methods for deducting banknotes issued as change from the database
/// </summary>
public interface IDecrementAvailableNotes
{
    /// <summary>
    /// Method for deducting banknotes issued as change from the database
    /// </summary>
    /// <param name="change"> </param>
    /// <exception cref="Exception"> </exception>
    Task DecrementAvailableNoteAsync(List<uint> change);
}