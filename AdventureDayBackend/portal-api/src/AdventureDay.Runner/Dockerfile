FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

COPY AdventureDay.DataModel /AdventureDay.DataModel
COPY AdventureDay.Runner /AdventureDay.Runner
WORKDIR /AdventureDay.Runner
RUN dotnet publish -c Release -o out

# Build Runtime Image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /AdventureDay.Runner/out .

# ... and run as non-root user.
RUN chown -R www-data:www-data /app
USER www-data

ENTRYPOINT ["dotnet", "AdventureDay.Runner.dll"]
