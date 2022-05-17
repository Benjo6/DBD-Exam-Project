USER=$1
su $USER

echo "config setup 2"
echo 'sh.addShard("mongors1/mongo-shard1-1:27018")' | mongosh --quiet
echo 'sh.addShard("mongors2/mongo-shard2-1:27018")' | mongosh --quiet

echo "sh.status()" | mongosh --quiet

echo "**Setup Shards**"
echo 'sh.enableSharding("twitter")' | mongosh --quiet
echo 'sh.shardCollection("twitter.tweets", {"source" : "hashed"})' | mongosh --quiet



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
git clone https://github.com/danielgron/db_assignment3_mongodb_sharding.git /repo/
