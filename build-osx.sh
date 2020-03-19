#!/usr/bin/env bash

VERSION="$(date +'%Y.%m.%d').$(git rev-list --count HEAD)"
FILENAME="apm.OSX.$VERSION"

mkdir -p release-bin

cd AProjectManager.Cli

dotnet publish -c Release -r osx-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true

cp bin/Release/netcoreapp3.1/osx-x64/publish/AProjectManager.Cli ../release-bin/$FILENAME
