using System.ComponentModel.DataAnnotations;

namespace CoffeeMachine.Auth;

public class User
{
    public record UserDto(string UserName, string Password);

    public record UserModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}