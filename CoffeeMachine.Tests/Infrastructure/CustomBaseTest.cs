﻿namespace CoffeeMachine.Tests.Infrastructure;

public class CustomBaseTest
{
    private readonly CustomWebApplicationFactory _webApplicationFactory;
    public ExternalServicesMock ExternalServicesMock { get; }

    public CustomBaseTest()
    {
        ExternalServicesMock = new ExternalServicesMock();
        _webApplicationFactory = new CustomWebApplicationFactory(ExternalServicesMock);
    }

    public HttpClient GetClient() => _webApplicationFactory.CreateClient();
}