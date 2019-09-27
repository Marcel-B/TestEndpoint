FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 8045

FROM microsoft/dotnet:2.2-sdk AS build

#FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY TestPoint/TestPoint.csproj TestPoint/
COPY TestPoint/NuGet.config /TestPoint

RUN dotnet restore "TestPoint/TestPoint.csproj" --configfile NuGet.config

COPY . "TestPoint"
WORKDIR "/src/TestPoint"
RUN dotnet build "TestPoint.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestPoint.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestPoint.dll"]