$CONFIGURATION = $env:CONFIGURATION
$BUILD_FOLDER = $env:APPVEYOR_BUILD_FOLDER
$PROJECT = Join-Path $BUILD_FOLDER 'CloudPayments.Cash.Tests'

Write-Host "Packaging project" -ForegroundColor Cyan
Push-Location $PROJECT
& dotnet test
if($LASTEXITCODE) {
    exit 1
}
Pop-Location