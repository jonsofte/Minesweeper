# Base aspnet image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build API backend
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MinesweeperApi/MinesweeperApi.csproj", "MinesweeperApi/"]
RUN dotnet restore "MinesweeperApi/MinesweeperApi.csproj"
COPY . .
WORKDIR "/src/MinesweeperApi"
RUN dotnet build "MinesweeperApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinesweeperApi.csproj" -c Release -o /app/publish

# Build vue app - frontend
FROM node:lts-alpine AS vuebuild
WORKDIR /vueapp
COPY "../MinesweeperVue/package*.json" ./
RUN npm install
COPY "../MinesweeperVue" .
RUN npm run build

# Create release image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=vuebuild /vueapp/dist ./wwwroot/

CMD sed -i -e "s/{{ APPLICATION_INSIGHTS_KEY }}/$APPLICATION_INSIGHTS_KEY/g" /app/wwwroot/js/app.*.js && \
dotnet MinesweeperApi.dll