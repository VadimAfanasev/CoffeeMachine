namespace CoffeMachine.Common.Interfaces
{
    using CoffeMachine.Dto;

    public interface IIncrementMoneyInMachine
    {
        void IncrementMoney(List<InputMoneyDto> inputMoney);
    }
}