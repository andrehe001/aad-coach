FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

COPY AdventureDay.DataModel /AdventureDay.DataModel
COPY AdventureDay.PortalApi /AdventureDay.PortalApi
WORKDIR /AdventureDay.PortalApi
RUN dotnet publish -c Release -o out

# Build Runtime Image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /AdventureDay.PortalApi/out .

# ... and run as non-root user.
RUN chown -R www-data:www-data /app
USER www-data

ENV ASPNETCORE_URLS http://+:8080
ENTRYPOINT ["dotnet", "AdventureDay.PortalApi.dll"]
