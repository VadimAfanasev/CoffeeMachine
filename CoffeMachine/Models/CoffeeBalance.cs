using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoffeMachine.Models
{
    public class CoffeeBalance
    {
        [Key]
        public string Name { get; set; }
        public uint Balance { get; set; }
    }
}
