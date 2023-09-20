#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DistributedKeyValueStore.ApiGateway/DistributedKeyValueStore.ApiGateway.csproj", "src/DistributedKeyValueStore.ApiGateway/"]
RUN dotnet restore "src/DistributedKeyValueStore.ApiGateway/DistributedKeyValueStore.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/DistributedKeyValueStore.ApiGateway"
RUN dotnet build "DistributedKeyValueStore.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DistributedKeyValueStore.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DistributedKeyValueStore.ApiGateway.dll"]