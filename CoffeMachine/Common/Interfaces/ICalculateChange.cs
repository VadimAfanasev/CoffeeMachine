namespace CoffeeMachine.Common.Interfaces
{
    public interface ICalculateChange
    {
        Task<List<uint>> CalculateAsync(uint amount);
    }
}