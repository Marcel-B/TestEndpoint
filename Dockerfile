FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
ARG var_name 
ENV env_var_name =$var_name
WORKDIR /app
EXPOSE 8045

FROM microsoft/dotnet:2.2-sdk AS build

RUN echo $evn_name
#FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
RUN ls
COPY "TestPoint.csproj" "./TestPoint/TestPoint.csproj"
COPY "TestPoint/NuGet.config" "./TestPoint/NuGet.config"


RUN dotnet restore "./TestPoint/TestPoint.csproj" --configfile "./TestPoint/NuGet.config"

COPY . "./TestPoint"
WORKDIR "/src/TestPoint"



RUN ls


RUN dotnet build "TestPoint.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestPoint.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestPoint.dll"]