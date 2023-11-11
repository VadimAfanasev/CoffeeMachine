namespace CoffeeMachine.Tests.Infrastructure;

public class CustomBaseTest
{
    private CustomWebApplicationFactory _webApplicationFactory;
    private ExternalServicesMock ExternalServicesMock { get; set;}

    public CustomBaseTest()
    {
        ExternalServicesMock = new ExternalServicesMock();
    }

    [SetUp]
    public void Initialize()
    {
        
        _webApplicationFactory = new CustomWebApplicationFactory(ExternalServicesMock);
    }

    [TearDown]
    public void TearDown()
    {
        _webApplicationFactory.Dispose();
    }
    public HttpClient GetClient() => _webApplicationFactory.CreateClient();
}