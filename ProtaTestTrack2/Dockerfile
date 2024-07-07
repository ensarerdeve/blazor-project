FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProtaTestTrack2.csproj", "./"]
RUN dotnet restore "ProtaTestTrack2.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ProtaTestTrack2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProtaTestTrack2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProtaTestTrack2.dll"]
