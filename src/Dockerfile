ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY ["Rise.Server/Rise.Server.csproj", "Rise.Server/"]
RUN dotnet restore "./Rise.Server/Rise.Server.csproj"
COPY . .
WORKDIR "/src/Rise.Server"
RUN dotnet build "./Rise.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
ARG API_BASE_URL
RUN dotnet publish "./Rise.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish
RUN if [ -n "$API_BASE_URL" ]; then \
      sed -i "s|https://localhost:5001|$API_BASE_URL|g" /app/publish/wwwroot/appsettings.json; \
    fi

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS server
USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rise.Server.dll"]

FROM nginx:alpine AS client
WORKDIR /app
COPY --from=publish /app/publish .
COPY Rise.Client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
EXPOSE 443
