# build.ps1

$ErrorActionPreference = "Stop"

Write-Host "=== Building NuGet package KassEvents.Contracts ==="

# Build package to shared packages folder
dotnet pack KassEvents.Contracts/KassEvents.Contracts.csproj `
    -c Release `
    -o ./packages

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to build package" -ForegroundColor Red
    exit 1
}

Write-Host "Package built in ./packages"

# Copy packages to service folders
$services = @("KassaEventSimulator", "KassaReaderService")

foreach ($service in $services) {
    if (Test-Path $service) {
        Write-Host "Copying packages to $service/packages/"
        New-Item -ItemType Directory -Force -Path "$service/packages" | Out-Null
        Copy-Item -Force ./packages/*.nupkg "$service/packages/"
    }
}

Write-Host "=== Starting docker compose ==="
docker compose up --build -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "Done" -ForegroundColor Green
}
else {
    Write-Host "ERROR: docker compose failed" -ForegroundColor Red
    exit 1
}