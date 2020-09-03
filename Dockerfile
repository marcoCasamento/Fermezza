#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Fermezza.csproj", ""]
RUN dotnet restore "./Fermezza.csproj"
COPY . .
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install
WORKDIR "/src/."
RUN dotnet build "Fermezza.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fermezza.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fermezza.dll"]
