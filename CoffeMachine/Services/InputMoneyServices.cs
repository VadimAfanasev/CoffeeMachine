using CoffeMachine.Dto;
using CoffeMachine.Models.Data;
using CoffeMachine.Services.Interfaces;
using System.Linq;

namespace CoffeMachine.Services
{
    public class InputMoneyServices: IInputMoneyServices
    {
        private CoffeeContext _db;
        private uint[] banknotes = new uint[] { 5000, 2000, 1000, 500 };
        public InputMoneyServices(CoffeeContext db)
        {
            _db = db;
        }
        public List<uint> InputingMoney(List<InputMoneyDto> inputMoney)
        {
            if (!inputMoney.All(c => banknotes.Contains(c.Nominal)));
            {
                //return BadRequest("Invalid banknotes type");
            }
        }
    }
}
