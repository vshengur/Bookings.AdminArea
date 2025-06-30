# Скрипт для применения миграций PostgreSQL
# Использование: .\migrate-up.ps1

param(
    [string]$ConnectionString = "Host=localhost;Database=bookings_db;Username=bookings_user;Password=bookings_password"
)

Write-Host "Применение миграций PostgreSQL..." -ForegroundColor Green

# Переходим в директорию с проектом PostgreSQL репозиториев
Set-Location "Bookings.PostgresRepositories"

try {
    # Применяем миграции
    dotnet ef database update --connection "$ConnectionString"
    
    Write-Host "Миграции успешно применены!" -ForegroundColor Green
}
catch {
    Write-Host "Ошибка при применении миграций: $_" -ForegroundColor Red
}
finally {
    # Возвращаемся в корневую директорию
    Set-Location ".."
} 