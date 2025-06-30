# Скрипт для отката миграций PostgreSQL
# Использование: .\migrate-down.ps1 [количество_миграций_для_отката]

param(
    [int]$Steps = 1,
    [string]$ConnectionString = "Host=localhost;Database=bookings_db;Username=bookings_user;Password=bookings_password"
)

Write-Host "Откат миграций PostgreSQL на $Steps шагов..." -ForegroundColor Yellow

# Переходим в директорию с проектом PostgreSQL репозиториев
Set-Location "Bookings.PostgresRepositories"

try {
    # Откатываем миграции
    dotnet ef database update --connection "$ConnectionString" -- $Steps
    
    Write-Host "Миграции успешно откачены!" -ForegroundColor Green
}
catch {
    Write-Host "Ошибка при откате миграций: $_" -ForegroundColor Red
}
finally {
    # Возвращаемся в корневую директорию
    Set-Location ".."
} 