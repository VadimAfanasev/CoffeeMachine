namespace CoffeMachine.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CoffeeBalance
    {
        public uint Balance { get; set; }

        [Key]
        public string Name { get; set; }
    }
}