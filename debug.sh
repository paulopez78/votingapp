#!/bin/bash
set -e
dotnet publish -c Debug -o bin/PublishOutput
docker rm -f $(docker ps -aq) > /dev/null 2>&1 || true
docker-compose -f docker-compose.yml -f docker-compose.debug.yml up --force-recreate