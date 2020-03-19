$VERSION="$(Get-Date -Format yyyy-MM-dd).$(git rev-list --count HEAD)"
$FILENAME="apm.windows.$VERSION.exe"
$BINPATH="release-bin"

If(!(test-path $BINPATH))
{
      New-Item -ItemType Directory -Force -Path $path
}

Set-Location AProjectManager.Cli

dotnet publish -c Release -r win10-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true

Copy-Item bin/Release/netcoreapp3.1/win10-x64/publish/AProjectManager.Cli.exe ../release-bin/$FILENAME

Set-Location ..