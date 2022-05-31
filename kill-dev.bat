docker compose -f infrastructure/docker-compose.applications.dev.yml down --remove-orphans
docker compose -f infrastructure/docker-compose.infrastructure.dev.yml down --remove-orphans
docker compose -f infrastructure/docker-compose.tools.yml down --remove-orphans

docker volume rm infrastructure_postgres_data
docker volume rm infrastructure_mongo_router_data
docker volume rm infrastructure_mongo_s1r1_data
docker volume rm infrastructure_mongo_s1r2_data
docker volume rm infrastructure_mongo_s2r1_data
docker volume rm infrastructure_mongo_s2r2_data
