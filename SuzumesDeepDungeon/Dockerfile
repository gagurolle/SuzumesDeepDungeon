# Используем многостадийную сборку
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Копируем только файл проекта для восстановления зависимостей
COPY ["SuzumesDeepDungeon.csproj", "."]
RUN dotnet restore "SuzumesDeepDungeon.csproj"

# 2. Копируем остальные файлы проекта
COPY . .

# 3. Собираем и публикуем проект в одном этапе
RUN dotnet publish "SuzumesDeepDungeon.csproj" -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Создаем пользователя и группу для безопасности
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

# Настройка томов для данных
VOLUME /app/data
VOLUME /app/logs

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Добавляем скрипт для применения миграций
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
