docker compose -f infrastructure/docker-compose.applications.dev.yml down --remove-orphans
docker compose -f infrastructure/docker-compose.infrastructure.dev.yml down --remove-orphans
docker compose -f infrastructure/docker-compose.tools.yml down --remove-orphans