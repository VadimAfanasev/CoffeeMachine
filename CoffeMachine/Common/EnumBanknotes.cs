namespace CoffeeMachine.Common;

/// <summary>
/// Class listing banknotes available for insertion into the machine
/// </summary>
public abstract class EnumBanknotes
{
    /// <summary>
    /// List of banknote denominations available for calculation
    /// </summary>
    public enum Banknotes : uint
    {
        /// <summary>
        /// 5000 banknote
        /// </summary>
        FiveThousand = 5000,
        /// <summary>
        /// 2000 banknote
        /// </summary>
        TwoThousand = 2000,
        /// <summary>
        /// 1000 banknote
        /// </summary>
        OneThousand = 1000,
        /// <summary>
        /// 500 banknote
        /// </summary>
        FiveHundred = 500
    }
}