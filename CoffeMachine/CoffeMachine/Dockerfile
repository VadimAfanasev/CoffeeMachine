#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeMachine/CoffeeMachine.csproj", "CoffeMachine/"]
RUN dotnet restore "CoffeMachine/CoffeeMachine.csproj"
COPY . .
WORKDIR "/src/CoffeMachine"
RUN dotnet build "CoffeeMachine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeMachine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5110
ENV ConnectionStrings__DefaultConnection="Server=172.26.192.1;Port=5432;Database=TestDBNew;User Id=postgres;Password=Vadim696;"
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5110/tcp

ENTRYPOINT ["dotnet", "CoffeeMachine.dll"]