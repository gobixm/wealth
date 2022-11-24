FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Wealth/Wealth.Backend/Wealth.Backend.csproj", "Wealth/Wealth.Backend/"]
RUN dotnet restore "Wealth/Wealth.Backend/Wealth.Backend.csproj"
COPY . .
WORKDIR "/src/Wealth/Wealth.Backend"
RUN dotnet build "Wealth.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wealth.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.Backend.dll"]
