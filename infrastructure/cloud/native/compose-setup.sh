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
docker compose -f infrastructure/docker-compose.infrastructure.prod.yml up -d --build
docker compose -f infrastructure/docker-compose.tools.prod.yml up -d --build
docker compose -f infrastructure/docker-compose.applications.prod.yml up -d --build



# NGINX / certbot