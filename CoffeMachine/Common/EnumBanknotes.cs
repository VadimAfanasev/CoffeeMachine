namespace CoffeeMachine.Common;

/// <summary>
/// Class listing banknotes available for insertion into the machine
/// </summary>
public class EnumBanknotes
{
    /// <summary>
    /// List of banknote denominations available for calculation
    /// </summary>
    public enum Banknotes : uint
    {
        FiveThousand = 5000,
        TwoThousand = 2000,
        OneThousand = 1000,
        FiveHundred = 500
    }
}