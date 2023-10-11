using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Auth;

/// <summary>
/// Class describing the user
/// </summary>
public class User
{
    /// <summary>
    /// Identifying the user Dto
    /// </summary>
    /// <param name="UserName"> </param>
    /// <param name="Password"> </param>
    public record UserDto(string UserName, string Password);

    /// <summary>
    /// Identifying the UserModel
    /// </summary>
    public record UserModel
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
}