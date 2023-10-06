namespace CoffeMachine.Services.Interfaces
{
    using CoffeMachine.Dto;

    public interface IInputMoneyServices
    {
        void Inputing(List<InputMoneyDto> inputMoney);
    }
}