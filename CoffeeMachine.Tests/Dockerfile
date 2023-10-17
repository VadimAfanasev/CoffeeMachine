#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeMachine.Tests/CoffeeMachine.Tests.csproj", "CoffeeMachine.Tests/"]
COPY ["CoffeMachine/CoffeeMachine.csproj", "CoffeMachine/"]
RUN dotnet restore "CoffeeMachine.Tests/CoffeeMachine.Tests.csproj"
COPY . .
WORKDIR "/src/CoffeeMachine.Tests"
RUN dotnet build "CoffeeMachine.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeMachine.Tests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeMachine.Tests.dll"]