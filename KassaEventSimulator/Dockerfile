# ============ СБОРКА ============
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 0. Копируем конфигурацию NuGet и пакеты
COPY nuget.config .
COPY ../packages/ /packages/

# 1. Восстановление зависимостей (кешируется, пока csproj не менялся)
COPY *.csproj .
RUN dotnet restore

# 2. Копируем всё остальное и собираем
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# ============ ЗАПУСК ============
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

USER $APP_UID
ENTRYPOINT ["dotnet", "KassaEventSimulator.dll"]