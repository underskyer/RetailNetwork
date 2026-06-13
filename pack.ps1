# build.ps1

$ErrorActionPreference = "Stop"


if (Test-Path "packages") {
	New-Item -ItemType Directory -Force -Path "packages" | Out-Null
}
Remove-Item "packages/*" -Recurse -Force	

$libs = @("KassEvents.Contracts", "KassaEventsDatabase")
foreach ($lib in $libs) {
	# Build package to shared packages folder
	Write-Host "=== Building NuGet package $lib ==="
	dotnet clean $lib/$lib.csproj `
			-c Release
	dotnet build $lib/$lib.csproj `
			-c Release
	Write-Host "=== dotnet pack $lib/$lib.csproj ==="
	dotnet pack $lib/$lib.csproj `
			-c Release `
			-o ./packages

	Write-Host "=== clear cache ==="
	if (Test-Path "$env:USERPROFILE\.nuget\packages\$lib") {
		Remove-Item -Recurse -Force "$env:USERPROFILE\.nuget\packages\$lib"
	}
	
}


if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to build package" -ForegroundColor Red
    exit 1
}


# Copy packages to service folders
$services = @("KassaEventSimulator", "KassaReaderService", "Reports.Web")

foreach ($service in $services) {
		Write-Host "Copying packages to $service/packages/"
    if (Test-Path $service) {
        New-Item -ItemType Directory -Force -Path "$service/packages" | Out-Null
    }
		Remove-Item "$service/packages/*" -Recurse -Force	
		Copy-Item -Force ./packages/*.nupkg "$service/packages/"
}
