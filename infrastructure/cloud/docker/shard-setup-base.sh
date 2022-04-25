USER=$1
REPL=$2
HOSTNAME=$3
su $USER
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh ./get-docker.sh
sudo groupadd docker 
sudo usermod -aG docker $USER 
sudo newgrp docker
sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile && mkswap /swapfile 
sudo swapon /swapfile
sudo echo "\n/swapfile swap swap defaults 0 0" >> /etc/fstab
sudo mkdir /docker 
sudo mkdir /docker/data && chmod 777 /docker 
sudo curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/docker-compose.shard.yml -o /docker/docker-compose.yml 
sudo curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/twitter.json -o /docker/data/twitter.json 
sudo curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/tweets.bson -o /docker/data/twitter.bson
sudo curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-Linux-x86_64" -o /usr/local/bin/docker-compose 
sudo chmod +x /usr/local/bin/docker-compose
sudo chgrp docker /usr/local/bin/docker-compose
export REPL=$REPL
docker-compose -f /docker/docker-compose.yml up -d