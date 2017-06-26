$CONFIGURATION = $env:CONFIGURATION
$BUILD_VERSION = if($env:APPVEYOR_REPO_TAG_NAME) { $env:APPVEYOR_REPO_TAG_NAME } else { $env:APPVEYOR_BUILD_VERSION }
$BUILD_FOLDER = $env:APPVEYOR_BUILD_FOLDER

Write-Host "version: $BUILD_VERSION" -ForegroundColor Yellow
Write-Host "folder: $BUILD_FOLDER" -ForegroundColor Yellow

Write-Host "Restoring packages" -ForegroundColor Cyan
& dotnet restore $BUILD_FOLDER /p:Version=$BUILD_VERSION
if($LASTEXITCODE) {
    exit 1
}

Write-Host "Building project" -ForegroundColor Cyan
& dotnet build $BUILD_FOLDER -c $CONFIGURATION /p:Version=$BUILD_VERSION
if($LASTEXITCODE) {
    exit 1
}