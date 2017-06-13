docker rm -f $(docker ps -aq)
docker-compose -f docker-compose.yml -f docker-compose.ci.yml up --force-recreate --build