# Используем многостадийную сборку
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SuzumesDeepDungeon.csproj", "."]
RUN dotnet restore "SuzumesDeepDungeon.csproj"
RUN dotnet build "SuzumesDeepDungeon.csproj" -c Release -o /app/build

FROM build AS publish
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

COPY --from=publish /app/publish .

# Добавляем скрипт для применения миграций
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]