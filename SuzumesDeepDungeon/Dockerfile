# 1. Базовый образ для сборки и публикации
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только файл проекта для ускорения restore
COPY ["SuzumesDeepDungeon.csproj", "."]
RUN dotnet restore "./SuzumesDeepDungeon.csproj"

# Копируем остальные файлы
COPY . .

# Устанавливаем инструмент dotnet-ef
RUN dotnet tool install --global dotnet-ef --version 8.*

# Удаляем папку фронтенда, чтобы она не попала в финальный образ
RUN rm -rf DeepDungeonClient

# Сборка и публикация
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SuzumesDeepDungeon.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 2. Финальный образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем entrypoint скрипт и настраиваем права ДО смены пользователя
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Копируем исходники для миграций (требуются для dotnet-ef)
COPY --from=build /src ./src

# Копируем глобальные tools из build-стадии
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools

# Для безопасности: отдельный пользователь
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

# Том для данных и логов
VOLUME /app/data
VOLUME /app/logs

# Открываем порты
EXPOSE 8080

# Установите переменные окружения по умолчанию
ENV Jwt__Key=""
ENV rawgAPI=""
ENV PATH="$PATH:/root/.dotnet/tools"

# ENTRYPOINT
ENTRYPOINT ["/bin/sh", "/app/entrypoint.sh"]