namespace CoffeeMachine.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class InputMoneyDto
    {
        [Required]
        public uint Count { get; set; }
        [Required]
        public uint Nominal { get; set; }
    }
}