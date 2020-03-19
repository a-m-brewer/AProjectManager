#!/usr/bin/env bash

VERSION="$(date +'%Y.%m.%d').$(git rev-list --count HEAD)"
FILENAME="apm.linux.$VERSION"

mkdir -p release-bin

cd AProjectManager.Cli

dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true

cp bin/Release/netcoreapp3.1/linux-x64/publish/AProjectManager.Cli ../release-bin/$FILENAME
