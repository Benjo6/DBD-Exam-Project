#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Copy csproj and restore as distinct layers
COPY PrescriptionService/PrescriptionService.csproj /app/PrescriptionService/
COPY lib /app/lib/
WORKDIR /app/PrescriptionService
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish PrescriptionService -c Release -o out

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrescriptionService.dll"]