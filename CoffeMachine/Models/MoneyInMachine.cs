using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoffeMachine.Models
{
    public class MoneyInMachine
    {
        [Key]
        public uint Nominal {  get; set; }
        public uint Count {  get; set; }
    }
}
