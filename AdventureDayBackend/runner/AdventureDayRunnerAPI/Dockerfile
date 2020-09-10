FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Improve Docker build performance by caching dependencies in separate layer.
COPY ./*.sln ./
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*} && mv $file ${file%.*}; done
RUN dotnet restore

# Copy the actual projects
COPY . ./
RUN dotnet publish AdventureDayRunnerAPI -c Release -o out

# Build Runtime Image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

# Use a non-root port
ENV ASPNETCORE_URLS=http://+:8080

# ... and run as non-root user.
RUN chown -R www-data:www-data /app
USER www-data

ENTRYPOINT ["dotnet", "AdventureDayRunnerAPI.dll"]
