namespace CoffeeMachine.Auth;

/// <summary>
/// An interface that describes obtaining a valid user
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// The method by which we obtain a valid user for authentication
    /// </summary>
    /// <param name="userModel"> Model to search for a user in the database </param>
    /// <returns> UserDto </returns>
    /// <exception cref="Exception"> User not found </exception>
    Task<UserDto> GetUserAsync(UserModel userModel);
}