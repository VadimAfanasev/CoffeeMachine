namespace CoffeeMachine.Models;

using System.ComponentModel.DataAnnotations;

public class MoneyInMachine
{
    public uint Count { get; set; }

    [Key]
    public uint Nominal { get; set; }
}