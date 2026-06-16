. ".\publish-paks.ps1"

Write-Host "=== Starting docker compose ==="
docker compose up --build -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "Done" -ForegroundColor Green
}
else {
    Write-Host "ERROR: docker compose failed" -ForegroundColor Red
    exit 1
}