# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Neo4J/Neo4J.csproj /app/Neo4J/
COPY lib /app/lib/
WORKDIR /app/Neo4J
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish Neo4J -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/Neo4J/out .
ENTRYPOINT ["dotnet", "Neo4J.dll"]