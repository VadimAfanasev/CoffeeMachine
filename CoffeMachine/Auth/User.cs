namespace CoffeeMachine.Auth;

/// <summary>
/// Identifying the user Dto
/// </summary>
public class UserDto
{
    /// <summary>
    /// UserName for entries in UserDto
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// Password for entries in UserDto
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Dto for user data records
    /// </summary>
    /// <param name="userName"> UserName for entries in UserDto </param>
    /// <param name="password"> Password for entries in UserDto </param>
    public UserDto(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}