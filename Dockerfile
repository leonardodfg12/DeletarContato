# Use the official .NET image as a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the entire source code and restore dependencies
COPY . ./
RUN dotnet restore

# Build the project
RUN dotnet build --configuration Release --no-restore

# Publish the project
RUN dotnet publish --configuration Release --no-restore --output /app/publish

# Use the official .NET runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "DeletarContato.API.dll"]