namespace CoffeeMachine.Dto;

/// <summary>
/// Class describing the purchase of coffee Dto
/// </summary>
public class OrderCoffeeDto
{
    /// <summary>
    /// List of banknotes for change Dto
    /// </summary>
    public List<uint>? Change { get; set; }
}