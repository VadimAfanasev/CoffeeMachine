using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Dto
{
    public class BalanceCoffeeDto
    {
        [Required]
        public uint Balance { get; set; }
        [Required]
        public string Name { get; set; }
    }
}