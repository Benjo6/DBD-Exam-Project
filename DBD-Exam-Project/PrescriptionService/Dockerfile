#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PrescriptionService/PrescriptionService.csproj", "PrescriptionService/"]
RUN dotnet restore "PrescriptionService/PrescriptionService.csproj"
COPY . .
WORKDIR "/src/PrescriptionService"
RUN dotnet build "PrescriptionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrescriptionService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrescriptionService.dll"]