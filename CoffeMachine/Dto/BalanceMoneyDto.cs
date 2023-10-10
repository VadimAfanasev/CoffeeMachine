using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Dto;

public class BalanceMoneyDto
{
    [Required]
    public uint Count { get; set; }
    [Required]
    public uint Nominal { get; set; }
}