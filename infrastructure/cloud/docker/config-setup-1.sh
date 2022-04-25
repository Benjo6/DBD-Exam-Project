echo "config setup 1"
USER=$1

curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh ./get-docker.sh
sudo groupadd docker
sudo usermod -aG docker $USER && newgrp docker


sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile
sudo mkswap /swapfile
sudo swapon /swapfile
sudo echo "\n/swapfile swap swap defaults 0 0" >> /etc/fstab

sudo mkdir /docker
sudo mkdir /docker/data
sudo chmod 777 /docker
sudo chmod 777 /docker/data
curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/docker-compose.config.yml -o /docker/docker-compose.yml
curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/twitter.json -o /docker/data/twitter.json
curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/tweets.bson -o /docker/data/twitter.bson

sudo curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-Linux-x86_64" -o /usr/local/bin/docker-compose 
sudo chmod +x /usr/local/bin/docker-compose

sudo usermod -aG docker $USER
newgrp docker
sudo chgrp docker /usr/local/bin/docker-compose
docker-compose -f /docker/docker-compose.yml up -d


docker exec mongocfg1 bash -c "echo 'rs.initiate({_id: \"mongors1conf\",configsvr: true, members: [{ _id : 0, host : \"mongocfg1:27017\" },{ _id : 1, host : \"mongocfg2:27017\" }, { _id : 2, host : \"mongocfg3:27017\" }]})' | mongosh --quiet"
docker exec mongocfg1 bash -c "echo 'rs.status()' | mongosh --quiet"




