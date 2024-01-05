#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Bookings.Api/Bookings.Api.csproj", "Bookings.Api/"]
RUN dotnet restore "Bookings.Api/Bookings.Api.csproj"
COPY . .
WORKDIR "/src/Bookings.Api"
RUN dotnet build "Bookings.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookings.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookings.Api.dll"]