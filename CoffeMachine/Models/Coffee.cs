using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models
{
    public class Coffee
    {
        public int Id { get; set; }
        public uint Balance { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public uint Price { get; set; }
    }
}