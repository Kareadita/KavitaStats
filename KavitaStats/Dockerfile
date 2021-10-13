FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KavitaStats/KavitaStats.csproj", "KavitaStats/"]
RUN dotnet restore "KavitaStats/KavitaStats.csproj"
COPY . .
WORKDIR "/src/KavitaStats"
RUN dotnet build "KavitaStats.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KavitaStats.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KavitaStats.dll"]
