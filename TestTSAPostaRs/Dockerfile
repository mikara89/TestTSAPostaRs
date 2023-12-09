#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestTSAPostaRs/TestTSAPostaRs.csproj", "TestTSAPostaRs/"]
RUN dotnet restore "./TestTSAPostaRs/./TestTSAPostaRs.csproj"
COPY . .
WORKDIR "/src/TestTSAPostaRs"
RUN dotnet build "./TestTSAPostaRs.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestTSAPostaRs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add certificates to the container from certs folder and make them trusted
COPY ["TestTSAPostaRs/certs/root.crt", "/usr/local/share/ca-certificates/"]
COPY ["TestTSAPostaRs/certs/ca.crt", "/usr/local/share/ca-certificates/"]
USER root

RUN update-ca-certificates

ENTRYPOINT ["dotnet", "TestTSAPostaRs.dll"]