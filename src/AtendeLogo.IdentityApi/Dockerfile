# Base image for runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

# Copy project files and restore dependencies
COPY ["src/AtendeLogo.IdentityApi/AtendeLogo.IdentityApi.csproj", "AtendeLogo.IdentityApi/"]

#Common
COPY ["src/AtendeLogo.Common/AtendeLogo.Common.csproj", "AtendeLogo.Common/"]
#Core
COPY ["src/AtendeLogo.Application/AtendeLogo.Application.csproj", "AtendeLogo.Application/"]
COPY ["src/AtendeLogo.Domain/AtendeLogo.Domain.csproj", "AtendeLogo.Domain/"]
COPY ["src/AtendeLogo.SharedKernel/AtendeLogo.SharedKernel.csproj", "AtendeLogo.SharedKernel/"]

#Use cases
COPY ["src/AtendeLogo.UseCases/AtendeLogo.UseCases.csproj", "AtendeLogo.UseCases/"]
COPY ["src/AtendeLogo.UseCases.Shared/AtendeLogo.UseCases.Shared.csproj", "AtendeLogo.UseCases.Shared/"]

#Persistence
COPY ["src/AtendeLogo.Persistence.Common/AtendeLogo.Persistence.Common.csproj", "AtendeLogo.Persistence.Common/"]
COPY ["src/AtendeLogo.Persistence.Identity/AtendeLogo.Persistence.Identity.csproj", "AtendeLogo.Persistence.Identity/"]
COPY ["src/AtendeLogo.Persistence.Activity/AtendeLogo.Persistence.Activity.csproj", "AtendeLogo.Persistence.Activity/"]

#Presentation
COPY ["src/AtendeLogo.Presentation/AtendeLogo.Presentation.csproj", "AtendeLogo.Presentation/"]

#Infrastructure
COPY ["src/AtendeLogo.Infrastructure/AtendeLogo.Infrastructure.csproj", "AtendeLogo.Infrastructure/"]

#Aspire Defaults
COPY ["src/AtendeLogo.ServiceDefaults/AtendeLogo.ServiceDefaults.csproj", "AtendeLogo.ServiceDefaults/"]


RUN dotnet restore "./AtendeLogo.IdentityApi/AtendeLogo.IdentityApi.csproj"

# Copy source files and build
COPY . .
WORKDIR "src/AtendeLogo.IdentityApi"
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtendeLogo.IdentityApi.dll"]
