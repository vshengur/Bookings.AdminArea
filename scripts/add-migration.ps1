# Скрипт для создания новой миграции PostgreSQL
# Использование: .\add-migration.ps1 "название_миграции"

param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName,
    [string]$ConnectionString = "Host=localhost;Database=bookings_db;Username=bookings_user;Password=bookings_password"
)

Write-Host "Создание новой миграции: $MigrationName" -ForegroundColor Blue

# Переходим в директорию с проектом PostgreSQL репозиториев
Set-Location "Bookings.PostgresRepositories"

try {
    # Создаем новую миграцию
    dotnet ef migrations add $MigrationName --connection "$ConnectionString"
    
    Write-Host "Миграция '$MigrationName' успешно создана!" -ForegroundColor Green
}
catch {
    Write-Host "Ошибка при создании миграции: $_" -ForegroundColor Red
}
finally {
    # Возвращаемся в корневую директорию
    Set-Location ".."
} 