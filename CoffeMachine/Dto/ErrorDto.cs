using System.Text.Json;

namespace CoffeeMachine.Dto;

/// <summary>
/// Class describing errors Dto
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Error message Dto
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Error status code Dto
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Serializing an entity to a string
    /// </summary>
    /// <returns> string </returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}