FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
ARG var_name 
ENV env_var_name =$var_name
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

RUN echo $evn_name

WORKDIR /src
RUN ls
COPY "TestPoint.csproj" "./TestPoint/TestPoint.csproj"
COPY "NuGet.config" "./TestPoint/NuGet.config"

RUN dotnet restore "./TestPoint/TestPoint.csproj" --configfile "./TestPoint/NuGet.config"

COPY . "./TestPoint"
WORKDIR "/src/TestPoint"

RUN dotnet build "TestPoint.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestPoint.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestPoint.dll"]