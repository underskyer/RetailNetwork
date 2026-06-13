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
$index = 1
$generatedCount = 0

foreach ($migration in $migrations) {
    # Очищаем от пробелов, переносов строк и удаляем текст " (Pending)"
    $cleanLine = $migration.Trim().Replace("`r", "").Replace("`n", "")
    $curr = $cleanLine -replace '\s*\(Pending\)\s*', ''
    
    # Регулярное выражение для разделения таймстампа и имени
    if ($curr -match '^(\d+)_(.+)$') {
        $timestamp = $Matches[1]
        $name = $Matches[2]
        
        # Формируем числовой индекс с лидирующими нулями (0001, 0002, ...)
        $indexFormatted = "{0:D4}" -f $index
        # Имя файла в формате ch-migrate: 0001_name.up.sql
        $chFileName = "$indexFormatted`_$name.up.sql"
        $outputPath = Join-Path $migrationDir $chFileName
        
        Write-Host "Генерация шага: $chFileName" -ForegroundColor Green
        
        # Выгружаем изолированный SQL-скрипт для конкретного шага
        dotnet ef migrations script $prev $curr --output $outputPath --no-color
        
        # Дополнительно: убираем из сгенерированного файла упоминания __EFMigrationsHistory
        # (опционально, если используете ch-migrate вместо EF Core migrations history)
        $content = Get-Content -Path $outputPath -Raw
        $content = $content -replace 'INSERT INTO "__EFMigrationsHistory".*$', '' -replace 'CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory".*?;', ''
        $content = $content -replace '^[\s\r\n]*', ''  # удаляем пустые строки в начале
        Set-Content -Path $outputPath -Value $content -NoNewline
        
        $prev = $curr
        $index++
        $generatedCount++
    }
}

if ($generatedCount -gt 0) {
    Write-Host "Успешно сгенерировано миграций: $generatedCount в папку $migrationDir" -ForegroundColor Green
    Write-Host "Формат файлов: 0001_name.up.sql (ch-migrate)" -ForegroundColor Cyan
} else {
    Write-Host "Ошибка парсинга имен миграций." -ForegroundColor Red
}