# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Neo4jDataAnalyzer/DataAnalyzingService.csproj /app/DataAnalyzingService/
COPY lib /app/lib/
WORKDIR /app/DataAnalyzingService
RUN dotnet restore DataAnalyzingService.csproj

# Copy everything else and build
COPY ./Neo4jDataAnalyzer ./
RUN dotnet publish DataAnalyzingService.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/DataAnalyzingService/out .
ENTRYPOINT ["dotnet", "DataAnalyzingService.dll"]