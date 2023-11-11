using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Dto;

/// <summary>
/// A class describing money deposited into a coffee machine
/// </summary>
public class MoneyDto
{
    /// <summary>
    /// Number of bills Dto
    /// </summary>
    [Required]
    public uint Count { get; set; }

    /// <summary>
    /// Denomination of banknotes Dto
    /// </summary>
    [Required]
    public uint Nominal { get; set; }
}