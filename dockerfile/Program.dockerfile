# Use the .NET Core 3.1 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build

# Set the working directory
WORKDIR /app

# Copy everything into the image's filesystem
COPY . ./

# Restore any dependencies (via NuGet)
RUN dotnet restore

# Publish the application to the /out directory
RUN dotnet publish -c Release -o out

# Use the .NET Core 3.1 runtime as the final base image
FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine AS runtime

# Set the working directory
WORKDIR /app

# Copy the published app from the build image
COPY --from=build /app/out ./

# Define the entrypoint for the application
ENTRYPOINT ["dotnet", "your-assembly-name.dll"]

