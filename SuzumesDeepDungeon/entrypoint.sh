#!/bin/sh

# Применяем миграции к существующей БД
dotnet SuzumesDeepDungeon.dll --migrate

# Запускаем приложение
exec dotnet SuzumesDeepDungeon.dll