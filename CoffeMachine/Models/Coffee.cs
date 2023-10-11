using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Models;

/// <summary>
/// The class that defines the essence of coffee in the database
/// </summary>
public class Coffee
{
    /// <summary>
    /// Balance of purchased coffee
    /// </summary>
    public uint Balance { get; set; }

    /// <summary>
    /// ID of each entity
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of coffee
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Coffee price
    /// </summary>
    [Required]
    public uint Price { get; set; }
}