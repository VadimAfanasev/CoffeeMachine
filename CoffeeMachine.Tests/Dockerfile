#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeMachine.Tests/CoffeeMachine.Tests.csproj", "CoffeeMachine.Tests/"]
COPY ["CoffeMachine/CoffeeMachine.csproj", "CoffeMachine/"]
RUN dotnet restore "CoffeMachine/CoffeeMachine.csproj"
RUN dotnet restore "CoffeeMachine.Tests/CoffeeMachine.Tests.csproj"
COPY . .
WORKDIR "/src/CoffeeMachine.Tests"
RUN dotnet build "CoffeeMachine.Tests.csproj" -c Release 
#-o /app/build

FROM build AS testrunner
WORKDIR "/src/CoffeeMachine.Tests"
#RUN dotnet publish "CoffeeMachine.Tests.csproj" -c Release -o /app/publish /p:UseAppHost=false
CMD ["dotnet", "test", "-c","Release","--no-restore", "--no-build"]
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "CoffeeMachine.Tests.dll"]