using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Dto;

/// <summary>
/// Class describing the balance of coffee in a coffee machine Dto
/// </summary>
public class BalanceCoffeeDto
{
    /// <summary>
    /// Amount of coffee purchased Dto
    /// </summary>
    [Required]
    public uint Balance { get; set; }

    /// <summary>
    /// Name of coffee Dto
    /// </summary>
    [Required]
    public string Name { get; set; }
}