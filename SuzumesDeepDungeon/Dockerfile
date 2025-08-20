# 1. Базовый образ для сборки и публикации
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только файл проекта для ускорения restore
COPY ["SuzumesDeepDungeon.csproj", "."]
RUN dotnet restore "./SuzumesDeepDungeon.csproj"

# Копируем остальные файлы
COPY . .

# Удаляем папку фронтенда, чтобы она не попала в финальный образ
RUN rm -rf DeepDungeonClient

# Сборка и публикация
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SuzumesDeepDungeon.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

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

# Открываем порты
EXPOSE 8080

# Установите переменные окружения по умолчанию (пустые значения)
ENV Jwt__Key=""
ENV rawgAPI=""

# ENTRYPOINT
ENTRYPOINT ["dotnet", "SuzumesDeepDungeon.dll"]