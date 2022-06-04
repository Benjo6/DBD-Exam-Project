USER=$1
PASSWORD=$2
DB_USER=$3
DB_PASSWORD=$4
su $USER

# Install docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh ./get-docker.sh
sudo groupadd docker
sudo usermod -aG docker $USER && newgrp docker

# Install docker-compose
sudo curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-Linux-x86_64" -o /usr/local/bin/docker-compose 
sudo chmod +x /usr/local/bin/docker-compose

sudo usermod -aG docker $USER
newgrp docker
sudo chgrp docker /usr/local/bin/docker-compose

#Setup frontend and backend
sudo mkdir /repo && sudo chgrp docker /repo
git clone https://github.com/SOFT2022-Database-Exam/DBD-Exam-Project.git /repo/

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