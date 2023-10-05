namespace CoffeMachine.Common.Interfaces
{
    public interface ICalculateChange
    {
        List<uint> Calculate(uint amount);
    }
}