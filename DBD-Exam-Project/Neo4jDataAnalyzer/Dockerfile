# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Neo4jDataAnalyzer/Neo4jDataAnalyzer.csproj /app/Neo4jDataAnalyzer/
COPY lib /app/lib/
WORKDIR /app/Neo4jDataAnalyzer
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish Neo4jDataAnalyzer -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/Neo4jDataAnalyzer/out .
ENTRYPOINT ["dotnet", "Neo4jDataAnalyzer.dll"]