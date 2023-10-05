using CoffeMachine.Dto;

namespace CoffeMachine.Services.Interfaces
{
    public interface IInputMoneyServices
    {
        List<uint> InputingMoney(List<InputMoneyDto> inputMoney);
    }
}
