using System.Text.Json;

namespace CoffeeMachine.Dto
{
    public class ErrorDto
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}