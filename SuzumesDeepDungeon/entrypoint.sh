#!/bin/sh

# Ждем, пока PostgreSQL будет готова
echo "Waiting for PostgreSQL to be ready..."
sleep 5

echo "Checking if database exists..."
if ! PGPASSWORD="130171" psql -h db -U postgres -lqt | cut -d \| -f 1 | grep -qw gameDB; then
    echo "Database does not exist. Creating database..."
    PGPASSWORD="130171" createdb -h db -U postgres gameDB
    echo "Database created successfully."
else
    echo "Database already exists."
fi

# Применяем миграции
echo "Applying database migrations..."
dotnet ef database update --project /app/src/SuzumesDeepDungeon.csproj

# Запускаем приложение
echo "Starting application..."
exec dotnet /app/SuzumesDeepDungeon.dll