using CoffeeMachine.Common.Interfaces;
using CoffeeMachine.Dto;
using CoffeeMachine.Services.Interfaces;

namespace CoffeeMachine.Services
{
    public class InputMoneyServices : IInputMoneyServices
    {
        private readonly List<int> _banknotes = new List<int> { 5000, 2000, 1000, 500 };
        private readonly IIncrementMoneyInMachine _incrementMoneyInMachine;

        public InputMoneyServices(IIncrementMoneyInMachine incrementMoneyInMachine)
        {
            _incrementMoneyInMachine = incrementMoneyInMachine;
        }

        public async Task InputingAsync(List<InputMoneyDto> inputMoney)
        {
            if (!inputMoney.All(c => _banknotes.Contains(c.Nominal)))
            {
                throw new ArgumentException("Invalid banknotes type");
            }

            await _incrementMoneyInMachine.IncrementMoneyAsync(inputMoney);
        }
    }
}