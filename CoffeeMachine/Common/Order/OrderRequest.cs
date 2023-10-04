namespace CoffeeMachine.Common.Order
{
    public class OrderRequest
    {
        public List<uint> InputMoney { get; set; }
        //public uint[] InputMoney { get; set; }

        public long Amount => InputMoney.Sum(c => c);

    }
}
