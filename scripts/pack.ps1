$CONFIGURATION = $env:CONFIGURATION
$BUILD_VERSION = if($env:APPVEYOR_REPO_TAG) { $env:APPVEYOR_REPO_TAG_NAME } else { $env:APPVEYOR_BUILD_VERSION }
$BUILD_FOLDER = $env:APPVEYOR_BUILD_FOLDER
$PROJECT = Join-Path $BUILD_FOLDER 'CloudPayments.Cash'
$OUTDIR = Join-Path $BUILD_FOLDER 'artifacts'

Write-Host "Packaging project" -ForegroundColor Cyan
& dotnet pack $PROJECT -c $CONFIGURATION -o $OUTDIR /p:Version=$BUILD_VERSION
if($LASTEXITCODE) {
    exit 1
}