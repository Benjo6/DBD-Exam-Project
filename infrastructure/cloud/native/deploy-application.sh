USER=$1
TWITTER=$2

echo 'User = '$1
echo 'API key = '$2

su $1

sudo rm -r /repo
sudo mkdir /repo && sudo chgrp docker /repo
git clone https://github.com/danielgron/db_assignment3_mongodb_sharding.git /repo/
docker-compose -f /repo/docker-compose.application.yml down

echo 'TWITTER='$2 >> /repo/.env
docker-compose -f /repo/docker-compose.application.yml up -d --build

