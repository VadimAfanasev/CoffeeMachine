using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models
{
    public class MoneyInMachine
    {
        [Key]
        public uint Nominal { get; set; }
        public uint Count { get; set; }
    }
}
