# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rise.Client/Rise.Client.csproj", "Rise.Client/"]
RUN dotnet restore "./Rise.Client/Rise.Client.csproj"
COPY . .
WORKDIR "/src/Rise.Client"
RUN dotnet build "./Rise.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rise.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM nginx:alpine as final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
EXPOSE 443


ENTRYPOINT ["dotnet", "Rise.Client.dll"]