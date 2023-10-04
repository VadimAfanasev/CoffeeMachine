using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models
{
    public class CoffeeBalance
    {
        [Key]
        public string CoffeeName { get; set; }
        public uint Balance { get; set; }
    }
}
