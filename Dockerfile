# Base aspnet image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build .Net API - Backend
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/MinesweeperApi/Minesweeper.Api.csproj", "MinesweeperApi/"]
RUN dotnet restore "src/MinesweeperApi/Minesweeper.Api.csproj"
COPY . .
WORKDIR "/src/MinesweeperApi"
RUN dotnet build "Minesweeper.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Minesweeper.Api.csproj" -c Release -o /app/publish

# Build Vue Application - Frontend
FROM node:lts-alpine AS vuebuild
WORKDIR /vueapp
COPY "src/MinesweeperVue/package*.json" ./
RUN npm install
COPY "src/MinesweeperVue" .
RUN npm run build

# Create container image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=vuebuild /vueapp/dist ./wwwroot/

# Set Application Insights key - At container startup, replace key with value from runtime environment variable
CMD sed -i -e "s/{{ APPLICATION_INSIGHTS_KEY }}/$APPLICATION_INSIGHTS_KEY/g" /app/wwwroot/js/app.*.js && \
dotnet Minesweeper.Api.dll