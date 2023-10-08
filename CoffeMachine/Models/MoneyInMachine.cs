using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models
{
    public class MoneyInMachine
    {
        public uint Count { get; set; }

        [Key]
        public uint Nominal { get; set; }
    }
}