$CONFIGURATION = $env:CONFIGURATION
$BUILD_VERSION = ($env:APPVEYOR_BUILD_VERSION -split "-")[0]
$BUILD_FOLDER = $env:APPVEYOR_BUILD_FOLDER
$PROJECT = Join-Path $BUILD_FOLDER 'CloudPayments.Cash'
$OUTDIR = Join-Path $BUILD_FOLDER 'artifacts'

Write-Host "Packaging project" -ForegroundColor Cyan
& dotnet pack $PROJECT -c $CONFIGURATION -o $OUTDIR /p:Version=$BUILD_VERSION
if($LASTEXITCODE) {
    exit 1
}