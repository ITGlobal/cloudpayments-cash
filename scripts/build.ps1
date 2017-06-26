$CONFIGURATION = $env:CONFIGURATION
$BUILD_FOLDER = $env:APPVEYOR_BUILD_FOLDER
$PROJECT = Join-Path $BUILD_FOLDER 'CloudPayments.Cash'
$OUTDIR = Join-Path $BUILD_FOLDER 'artifacts'

Write-Host "Restoring packages" -ForegroundColor Cyan
& dotnet restore $BUILD_FOLDER
if($LASTEXITCODE) {
    exit 1
}

Write-Host "Building project" -ForegroundColor Cyan
& dotnet build $BUILD_FOLDER -c $CONFIGURATION
if($LASTEXITCODE) {
    exit 1
}