USER=$1
REPL=$2
su $USER
sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile && mkswap /swapfile 
sudo swapon /swapfile
sudo echo "\n/swapfile swap swap defaults 0 0" >> /etc/fstab
sudo mkdir /data && chmod 777 /data
curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/twitter.json -o /data/twitter.json 
curl https://raw.githubusercontent.com/danielgron/db_assignment3_mongodb_sharding/main/tweets.bson -o /data/twitter.bson

#sudo apt update
#sudo apt install ntp
#echo date
#sudo service ntp stop
#sudo ntpd -gq
#sudo service ntp start
#echo date >> /home/$USER/timelog

## Install Mongo ##
wget -qO - https://www.mongodb.org/static/pgp/server-5.0.asc | sudo apt-key add -
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/5.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-5.0.list
sudo apt-get update
sudo apt-get install -y mongodb-org=5.0.6 mongodb-org-database=5.0.6 mongodb-org-server=5.0.6 mongodb-org-shell=5.0.6 mongodb-org-mongos=5.0.6 mongodb-org-tools=5.0.6

mkdir -p /home/$USER/data/db
mongod --shardsvr --replSet ${REPL} --dbpath /home/$USER/data/db --port 27018 --bind_ip_all --fork --logpath=/home/$USER/mongo.log --logappend