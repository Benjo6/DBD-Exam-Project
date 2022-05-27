# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY CronJobService/CronJobService.csproj /app/CronJobService/
COPY lib /app/lib/
WORKDIR /app/CronJobService
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish CronJobService -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/CronJobService/out .
ENTRYPOINT ["dotnet", "CronJobService.dll"]