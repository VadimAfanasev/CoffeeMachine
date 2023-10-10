namespace CoffeeMachine.Common.Interfaces;

public interface IIncrementAvailableNotes
{
    Task IncrementAvailableNoteAsync(uint[] inputMoney);
}