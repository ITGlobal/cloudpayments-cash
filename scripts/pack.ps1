$CONFIGURATION = $env:CONFIGURATION
$BUILD_FOLDER = $env:APPVEYOR_BUILD_FOLDER
$PROJECT = Join-Path $BUILD_FOLDER 'CloudPayments.Cash'
$OUTDIR = Join-Path $BUILD_FOLDER 'artifacts'

Write-Host "Packaging project" -ForegroundColor Cyan
& dotnet pack $PROJECT -c $CONFIGURATION -o $OUTDIR
if($LASTEXITCODE) {
    exit 1
}