#!/bin/sh

# Ждем, пока PostgreSQL будет готова
echo "Waiting for PostgreSQL to be ready..."
sleep 5

# Применяем миграции
echo "Applying database migrations..."
dotnet ef database update --project /app/src/SuzumesDeepDungeon.csproj

# Запускаем приложение
echo "Starting application..."
exec dotnet /app/SuzumesDeepDungeon.dll