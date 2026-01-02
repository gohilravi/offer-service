# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy solution file first
COPY *.sln .

# Copy project files for restore
COPY src/OfferService.Domain/OfferService.Domain.csproj src/OfferService.Domain/
COPY src/OfferService.Application/OfferService.Application.csproj src/OfferService.Application/
COPY src/OfferService.Infrastructure/OfferService.Infrastructure.csproj src/OfferService.Infrastructure/
COPY src/OfferService.Api/OfferService.Api.csproj src/OfferService.Api/

# Clear any previous NuGet cache and restore with verbose logging
RUN dotnet nuget locals all --clear
RUN dotnet restore src/OfferService.Api/OfferService.Api.csproj --verbosity normal --no-cache

# Copy the rest of the source code (exclude tests)
COPY src/ src/

# Build the solution (this will restore if needed)
RUN dotnet build src/OfferService.Api/OfferService.Api.csproj -c Release

# Publish the API project
RUN dotnet publish src/OfferService.Api/OfferService.Api.csproj -c Release -o /app/publish

# Use the official .NET 8 runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Set the working directory
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Create a non-root user
RUN addgroup --system --gid 1001 appuser \
    && adduser --system --uid 1001 --gid 1001 --home /home/appuser appuser

# Change ownership of the app directory
RUN chown -R appuser:appuser /app
USER appuser

# Expose the port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "OfferService.Api.dll"]