# Принудительно очищаем или создаем чистую папку для миграций
$migrationDir = Join-Path $PSScriptRoot "sql-migrations"
if (Test-Path $migrationDir) {
    Remove-Item -Path $migrationDir -Recurse -Force
}
New-Item -ItemType Directory -Path $migrationDir -Force | Out-Null

Write-Host "Запрос списка миграций из EF Core..." -ForegroundColor Cyan

# Получаем список миграций
$migrations = dotnet ef migrations list --no-color | Where-Object { $_ -match '^\d+_' }

if ($null -eq $migrations -or $migrations.Count -eq 0) {
    Write-Host "Миграции не найдены в выводе dotnet ef." -ForegroundColor Yellow
    exit
}

$prev = "0"
$generatedCount = 0

foreach ($migration in $migrations) {
    # Очищаем от пробелов, переносов строк и удаляем текст " (Pending)"
    $cleanLine = $migration.Trim().Replace("`r", "").Replace("`n", "")
    $curr = $cleanLine -replace '\s*\(Pending\)\s*', ''
    
    # Регулярное выражение для разделения таймстампа и имени
    if ($curr -match '^(\d+)_(.+)$') {
        $timestamp = $Matches[1]
        $name      = $Matches[2]
        
        # Формируем имя файла под стандарт Flyway: V[timestamp]__[name].sql
        $flywayFileName = "V$($timestamp)__$($name).sql"
        $outputPath = Join-Path $migrationDir $flywayFileName
        
        Write-Host "Генерация шага: $flywayFileName" -ForegroundColor Green
        
        # Выгружаем изолированный SQL-скрипт для конкретного шага
        dotnet ef migrations script $prev $curr --output $outputPath --no-color
        
        $prev = $curr
        $generatedCount++
    }
}

if ($generatedCount -gt 0) {
    Write-Host "Успешно сгенерировано миграций: $generatedCount в папку KassaEventsDataBase/migrations" -ForegroundColor Green
} else {
    Write-Host "Ошибка парсинга имен миграций." -ForegroundColor Red
}
