using CoffeeMachine.Services.Interfaces;
using Moq;
using System.Reflection;

namespace CoffeeMachine.Tests.Infrastructure;

public class ExternalServicesMock
{
    //public Mock<CoffeeMachineBuyController> CoffeeMachineClient { get; }

    //public ExternalServicesMock()
    //{
    //    CoffeeMachineClient = new Mock<CoffeeMachineBuyController>();
    //}

    //public IEnumerable<(Type, object)> GetMocks()
    //{
    //    return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
    //        .Select(x =>
    //        {
    //            var underlyingType = x.PropertyType.GetGenericArguments()[0];
    //            var value = x.GetValue(this) as Mock;

    //            return (underlyingType, value.Object);
    //        })
    //        .ToArray();
    //}

    public Mock<ICoffeeBuyServices> CoffeeMachineApiClient { get; }

    public ExternalServicesMock()
    {
        CoffeeMachineApiClient = new Mock<ICoffeeBuyServices>();
    }

    public IEnumerable<(Type, object)> GetMocks()
    {
        return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x =>
            {
                var underlyingType = x.PropertyType.GetGenericArguments()[0];
                var value = x.GetValue(this);

                return (underlyingType, value);
            })
            .ToArray();
    }
}