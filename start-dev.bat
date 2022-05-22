docker-compose -f infrastructure/docker-compose.applications.dev.yml down --remove-orphans
docker-compose -f infrastructure/docker-compose.infrastructure.dev.yml down --remove-orphans


docker-compose -f infrastructure/docker-compose.infrastructure.dev.yml up --build -d
docker-compose -f infrastructure/docker-compose.applications.dev.yml up --build -d