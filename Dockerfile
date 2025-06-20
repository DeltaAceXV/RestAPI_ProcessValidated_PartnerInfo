# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8082
EXPOSE 8082

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RestAPI_ProcessValidated_PartnerInfo.csproj", "."]
RUN dotnet restore "./RestAPI_ProcessValidated_PartnerInfo.csproj"
COPY . .
RUN dotnet build "./RestAPI_ProcessValidated_PartnerInfo.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RestAPI_ProcessValidated_PartnerInfo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestAPI_ProcessValidated_PartnerInfo.dll"]