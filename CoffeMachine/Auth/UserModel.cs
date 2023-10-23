using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Auth;

/// <summary>
/// Identifying the UserModel
/// </summary>
public class UserModel
{
    /// <summary>
    /// Validity check password
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Username for validation
    /// </summary>
    [Required]
    public string UserName { get; set; }
}