FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
ARG DB_USER
ARG DB_PASSWORD
ARG DB_HOST
ARG DB_DATABASE

WORKDIR /src
COPY ["ProiectIS2/ProiectIS2.csproj", "ProiectIS2/"]
RUN dotnet restore "ProiectIS2/ProiectIS2.csproj"
COPY . .
WORKDIR "/src/ProiectIS2"

RUN echo "DB_USER=${DB_USER}" > .env \
    && echo "DB_PASSWORD=${DB_PASSWORD}" >> .env \
    && echo "DB_HOST=${DB_HOST}" >> .env \
    && echo "DB_DATABASE=${DB_DATABASE}" >> .env \
    && echo "Generated .env file with args" \
    
RUN dotnet build "./ProiectIS2.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProiectIS2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS finald
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/ProiectIS2/.env .
ENTRYPOINT ["dotnet", "ProiectIS2.dll"]
