# 1. Базовый образ для сборки и публикации
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только файл проекта для ускорения restore
COPY ["SuzumesDeepDungeon/SuzumesDeepDungeon.csproj", "SuzumesDeepDungeon/"]
RUN dotnet restore "./SuzumesDeepDungeon/SuzumesDeepDungeon.csproj"

# Копируем остальные файлы
COPY . .

# Сборка и публикация (режим определяется переменной BUILD_CONFIGURATION)
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SuzumesDeepDungeon/SuzumesDeepDungeon.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 2. Финальный образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Для безопасности: отдельный пользователь
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

# Том для данных и логов
VOLUME /app/data
VOLUME /app/logs

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Открываем порты для локального дебага и продакшна
EXPOSE 8080
EXPOSE 8081

# ENTRYPOINT для Visual Studio/CI/CD
ENTRYPOINT ["dotnet", "SuzumesDeepDungeon.dll"]
