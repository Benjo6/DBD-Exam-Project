docker compose -f infrastructure/docker-compose.applications.dev.yml down --remove-orphans
docker compose -f infrastructure/docker-compose.infrastructure.dev.yml down --remove-orphans
docker compose -f infrastructure/docker-compose.tools.yml down --remove-orphans

docker compose -f infrastructure/docker-compose.infrastructure.dev.yml up --build -d
docker compose -f infrastructure/docker-compose.applications.dev.yml up --build -d
docker compose -f infrastructure/docker-compose.tools.yml up -d



docker compose -f infrastructure/docker-compose.infrastructure.dev.yml sleep 5 && exec mongo-conf1 mongosh --quiet --file /scripts/setup_conf.js

docker compose -f infrastructure/docker-compose.infrastructure.dev.yml exec mongo-shard1-rs1 mongosh --quiet --file /scripts/setup_shard1.js
docker compose -f infrastructure/docker-compose.infrastructure.dev.yml exec mongo-shard2-rs1 mongosh --quiet --file /scripts/setup_shard2.js

docker compose -f infrastructure/docker-compose.infrastructure.dev.yml exec mongos mongosh --quiet --file /scripts/setup_mongos.js