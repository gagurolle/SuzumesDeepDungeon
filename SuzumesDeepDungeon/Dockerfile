# Используем многостадийную сборку
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Копируем только файл проекта для восстановления зависимостей
COPY ["SuzumesDeepDungeon.csproj", "."]
RUN dotnet restore "SuzumesDeepDungeon.csproj"

# 2. Копируем остальные файлы проекта
COPY . .

# 3. Собираем и публикуем проект
RUN dotnet publish "SuzumesDeepDungeon.csproj" -c Release -o /app/publish

# 4. Копируем и настраиваем entrypoint.sh на этапе build (где есть root-права)
COPY entrypoint.sh /app/publish/
RUN chmod +x /app/publish/entrypoint.sh

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Создаем пользователя и группу для безопасности
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

# Настройка томов для данных
VOLUME /app/data
VOLUME /app/logs

# Копируем опубликованные файлы (включая entrypoint.sh)
COPY --from=build /app/publish .

# Запускаем скрипт (уже исполняемый)
ENTRYPOINT ["./entrypoint.sh"]
