echo "config setup 1"
USER=$1
PASSWORD=$2

DB_USER=$3
DB_PASSWORD=$4

su $USER

sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile
sudo mkswap /swapfile
sudo swapon /swapfile
sudo echo "\n/swapfile swap swap defaults 0 0" >> /etc/fstab

sudo mkdir /data
sudo chmod 777 /data

mkdir -p /home/$USER/data/db-cfg1
mkdir -p /home/$USER/data/db-cfg2
mkdir -p /home/$USER/data/db-cfg3

sudo apt update
sudo apt install git

wget -qO - https://www.mongodb.org/static/pgp/server-5.0.asc | sudo apt-key add -
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/5.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-5.0.list
sudo apt-get update
sudo apt-get install -y mongodb-org=5.0.6 mongodb-org-database=5.0.6 mongodb-org-server=5.0.6 mongodb-org-shell=5.0.6 mongodb-org-mongos=5.0.6 mongodb-org-tools=5.0.6

## Start Mongo ##
mongod --configsvr --replSet mongors1conf --dbpath /home/$USER/data/db-cfg1 --port 27014 --bind_ip_all --fork --logpath=/home/$USER/mongocfg1.log --logappend
mongod --configsvr --replSet mongors1conf --dbpath /home/$USER/data/db-cfg2 --port 27015 --bind_ip_all --fork --logpath=/home/$USER/mongocfg2.log --logappend
mongod --configsvr --replSet mongors1conf --dbpath /home/$USER/data/db-cfg3 --port 27016 --bind_ip_all --fork --logpath=/home/$USER/mongocfg3.log --logappend

#mongocfg1 
sleep 20

echo 'rs.initiate({_id: "mongors1conf",configsvr: true, members: [{ _id : 0, host : "mongo-config:27014" },{ _id : 1, host : "mongo-config:27015" }, { _id : 2, host : "mongo-config:27016" }]})' | mongosh --port 27014 --quiet
echo 'rs.status()' | mongosh --port 27014 --quiet

mongos --configdb mongors1conf/mongo-config:27014,mongo-config:27015,mongo-config:27016 --port 27017 --bind_ip_all --fork --logpath=/home/$USER/mongo.log --logappend

echo @'db.createUser(
  {
    user: "'$DB_USER'",
    pwd: "'$DB_PASSWORD'",
    roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]
  })' | mongosh --port 27017 --quiet








