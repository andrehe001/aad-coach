#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TeamGameHub.GameEngine.Sidecar.csproj", "Sidecar/"]
RUN dotnet restore "Sidecar/TeamGameHub.GameEngine.Sidecar.csproj"
COPY . Sidecar/
WORKDIR "/src/Sidecar"
RUN dotnet build "TeamGameHub.GameEngine.Sidecar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamGameHub.GameEngine.Sidecar.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamGameHub.GameEngine.Sidecar.dll"]
