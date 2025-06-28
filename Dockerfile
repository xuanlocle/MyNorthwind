# Use the official .NET 9 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["MyNorthwind/MyNorthwind.csproj", "MyNorthwind/"]
RUN dotnet restore "MyNorthwind/MyNorthwind.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/MyNorthwind"

# Build the application
RUN dotnet build "MyNorthwind.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MyNorthwind.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for Cloud Run
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Create a non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Start the application
ENTRYPOINT ["dotnet", "MyNorthwind.dll"] 