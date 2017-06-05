#!/bin/bash
set -e
docker rm -f $(docker ps -aq) > /dev/null 2>&1 || true
docker-compose -f docker-compose.yml -f docker-compose.ci.yml up --force-recreate --build