
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers
COPY "Frontend/Frontend.csproj" "/app/Frontend/"
COPY lib /app/lib/
WORKDIR /app/Frontend
RUN dotnet restore "Frontend.csproj"

# Copy everything else and build
COPY ./Frontend ./
RUN dotnet publish "Frontend.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/Frontend/out .

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "Frontend.dll"]