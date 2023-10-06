namespace CoffeMachine.Services
{
    using CoffeMachine.Common.Interfaces;
    using CoffeMachine.Dto;
    using CoffeMachine.Services.Interfaces;

    public class InputMoneyServices : IInputMoneyServices
    {
        private readonly List<int> _banknotes = new List<int> { 5000, 2000, 1000, 500 };
        private readonly IIncrementMoneyInMachine _incrementMoneyInMachine;

        public InputMoneyServices(IIncrementMoneyInMachine incrementMoneyInMachine)
        {
            _incrementMoneyInMachine = incrementMoneyInMachine;
        }

        public void Inputing(List<InputMoneyDto> inputMoney)
        {
            if (!inputMoney.All(c => _banknotes.Contains(c.Nominal)))
            {
                throw new ArgumentException("Invalid banknotes type");
            }

            _incrementMoneyInMachine.IncrementMoney(inputMoney);
        }
    }
}