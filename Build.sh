#!/bin/bash

git rm -r --cached .database/** -f
git rm -r --cached ./app/** -f

docker-compose up --build &
sleep 1

dotnet ef database update

sudo dotnet run -c Development
#sudo dotnet build "NotifyServer.csproj" -c Release
#sudo dotnet publish "NotifyServer.csproj" -c Release
#sudo dotnet app/publish/NotifyServer.dll

docker stop NotifyServerSql
docker rm NotifyServerSql
