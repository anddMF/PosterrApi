FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# Copy the .NET project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the remaining files and build the API
COPY . .
RUN dotnet publish -c Release -o /app

# Use the .NET runtime image to run the API
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Posterr.API.dll"]