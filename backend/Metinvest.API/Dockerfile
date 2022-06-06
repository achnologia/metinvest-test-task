FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Metinvest.API/Metinvest.API.csproj", "Metinvest.API/"]
COPY ["Metinvest.Infrastructure/Metinvest.Infrastructure.csproj", "Metinvest.Infrastructure/"]
COPY ["Metinvest.Domain/Metinvest.Domain.csproj", "Metinvest.Domain/"]
COPY ["Metinvest.Application/Metinvest.Application.csproj", "Metinvest.Application/"]
RUN dotnet restore "Metinvest.API/Metinvest.API.csproj"
COPY . .
WORKDIR "/src/Metinvest.API"
RUN dotnet build "Metinvest.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Metinvest.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Metinvest.API.dll"]
