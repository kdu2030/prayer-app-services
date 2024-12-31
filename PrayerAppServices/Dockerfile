# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PrayerAppServices/PrayerAppServices.csproj", "PrayerAppServices/"]
RUN dotnet restore "./PrayerAppServices/PrayerAppServices.csproj"
COPY . .
WORKDIR "/src/PrayerAppServices"
RUN dotnet build "./PrayerAppServices.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Install dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PrayerAppServices.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /root/.dotnet/tools/dotnet-ef /usr/local/bin/dotnet-ef
ENTRYPOINT dotnet-ef database update -- --connection=$CONNECTION_STRINGS__DEFAULTCONNECTION && dotnet PrayerAppServices.dll