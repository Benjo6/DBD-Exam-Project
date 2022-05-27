# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY MailService/MailService.csproj /app/MailService/
COPY lib /app/lib/
WORKDIR /app/MailService
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish MailService -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/MailService/out .
ENTRYPOINT ["dotnet", "MailService.dll"]