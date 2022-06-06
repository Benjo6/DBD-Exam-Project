USER=$1
PASSWORD=$2
DB_USER=$3
DB_PASSWORD=$4

su $USER



echo 'ADMIN_USER='$1 >> /repo/.env
echo 'ADMIN_PASSWORD='$2 >> /repo/.env
echo 'DB_USER='$3 >> /repo/.env
echo 'DB_PASSWORD='$4 >> /repo/.env

cd /repo
git checkout release/exam-dsg

sudo sed -i -e 's/prescriptionserviceuser/'$DB_USER'/' -e 's/prescreptionservicepw/'$DB_PASSWORD'/' /repo/infrastructure/postgres/init.sql

docker compose -f infrastructure/docker-compose.infrastructure.prod.yml --env-file ./.env up -d --build
docker compose -f infrastructure/docker-compose.tools.prod.yml --env-file ./.env up -d --build
docker compose -f infrastructure/docker-compose.applications.prod.yml --env-file ./.env up -d --build



# NGINX / certbot