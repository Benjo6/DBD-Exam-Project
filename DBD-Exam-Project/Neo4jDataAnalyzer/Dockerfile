#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Neo4jDataAnalyzer/Neo4jDataAnalyzer.csproj", "Neo4jDataAnalyzer/"]
RUN dotnet restore "Neo4jDataAnalyzer/Neo4jDataAnalyzer.csproj"
COPY . .
WORKDIR "/src/Neo4jDataAnalyzer"
RUN dotnet build "Neo4jDataAnalyzer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neo4jDataAnalyzer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neo4jDataAnalyzer.dll"]