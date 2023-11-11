using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models;

/// <summary>
/// Class describing money in a coffee machine
/// </summary>
public class MoneyInMachine
{
    /// <summary>
    /// Number of bills
    /// </summary>
    public uint Count { get; set; }

    /// <summary>
    /// Nominal of banknotes
    /// </summary>
    [Key]
    public uint Nominal { get; set; }
}