FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
ARG var_name 
ENV env_var_name =$var_name
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

RUN echo $evn_name

WORKDIR /src
# Prevent 'Warning: apt-key output should not be parsed (stdout is not a terminal)'
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=1

# install NodeJS 13.x
# see https://github.com/nodesource/distributions/blob/master/README.md#deb
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs



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