namespace CoffeeMachine.Auth;

/// <summary>
/// An interface that describes obtaining a valid user
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// The method by which we obtain a valid user for authentication
    /// </summary>
    /// <param name="userModel"> </param>
    /// <returns> UserDto </returns>
    /// <exception cref="Exception"> </exception>
    Task<User.UserDto> GetUserAsync(User.UserModel userModel);
}