# Use the official .NET 8.0 SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

# **Ensure the 'data' directory is copied correctly**
COPY --from=build /app/Data ./Data

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "RecipeAPI.dll"]