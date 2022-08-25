param ($version='latest')

$currentFolder = $PSScriptRoot
$slnFolder = Join-Path $currentFolder "../../"

$dbMigratorFolder = Join-Path $slnFolder "src/FileUpload.DbMigrator"

Write-Host "********* BUILDING DbMigrator *********" -ForegroundColor Green
Set-Location $dbMigratorFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t mycompanyname/fileupload-db-migrator:$version .


$blazorFolder = Join-Path $slnFolder "src/FileUpload.Blazor"
$hostFolder = Join-Path $slnFolder "src/FileUpload.HttpApi.Host"

Write-Host "********* BUILDING Blazor Application *********" -ForegroundColor Green
Set-Location $blazorFolder
dotnet publish -c Release
docker build -f Dockerfile.local -t mycompanyname/fileupload-blazor:$version .





### ALL COMPLETED
Write-Host "COMPLETED" -ForegroundColor Green
Set-Location $currentFolder