#!/bin/bash
set -e
docker rm -f $(docker ps -aq) > /dev/null 2>&1 || true
docker run -d --name eventstore-node -it -p 2113:2113 -p 1113:1113 eventstore/eventstore
dotnet run