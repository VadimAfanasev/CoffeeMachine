namespace CoffeeMachine.Common
{
    public class EnumBanknotes
    {
        // Перечисление доступных для расчета номиналов купюр
        public enum Banknotes : uint
        {
            FiveThousand = 5000,
            TwoThousand = 2000,
            OneThousand = 1000,
            FiveHundred = 500
        }
    }
}
