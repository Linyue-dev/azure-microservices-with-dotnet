# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Wpm.Management.Api/Wpm.Management.Api.csproj", "Wpm.Management.Api/"]
RUN dotnet restore "Wpm.Management.Api/Wpm.Management.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Wpm.Management.Api"
RUN dotnet build "Wpm.Management.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Wpm.Management.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wpm.Management.Api.dll"]