namespace CoffeeMachine.Common.Interfaces;

public interface IDecrementAvailableNotes
{
    Task DecrementAvailableNoteAsync(List<uint> change);
}