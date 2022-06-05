USER=$1
PASSWORD=$2

DB_USER=$3
DB_PASSWORD=$4

HOSTNAME=$5

[[ -z "$USER" ]] && { echo "Error: USER not found"; exit 1; }
[[ -z "$PASSWORD" ]] && { echo "Error: PASSWORD not found"; exit 1; }
[[ -z "$DB_USER" ]] && { echo "Error: DB_USER not found"; exit 1; }
[[ -z "$DB_ADMIN" ]] && { echo "Error: DB_ADMIN not found"; exit 1; }
[[ -z "$HOSTNAME" ]] && { echo "Error: HOSTNAME not found"; exit 1; }


su $USER

echo $USER >> ~/deploy.log
echo $PASSWORD >> ~/deploy.log
echo $DB_USER >> ~/deploy.log
echo $DB_PASSWORD >> ~/deploy.log
echo $HOSTNAME >> ~/deploy.log

echo "Creating swap" >> ~/deploy.log
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

echo "Installing git"
sudo apt update
sudo apt install git

echo "Installing Mongo"
wget -qO - https://www.mongodb.org/static/pgp/server-5.0.asc | sudo apt-key add -
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/5.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-5.0.list
sudo apt-get update
sudo apt-get install -y mongodb-org=5.0.6 mongodb-org-database=5.0.6 mongodb-org-server=5.0.6 mongodb-org-shell=5.0.6 mongodb-org-mongos=5.0.6 mongodb-org-tools=5.0.6

echo "Starting Mongo"
## Start Mongo ##
mongod --configsvr --replSet mongors1conf --dbpath /home/$USER/data/db-cfg1 --port 27014 --bind_ip_all --fork --logpath=/home/$USER/mongocfg1.log --logappend
mongod --configsvr --replSet mongors1conf --dbpath /home/$USER/data/db-cfg2 --port 27015 --bind_ip_all --fork --logpath=/home/$USER/mongocfg2.log --logappend
mongod --configsvr --replSet mongors1conf --dbpath /home/$USER/data/db-cfg3 --port 27016 --bind_ip_all --fork --logpath=/home/$USER/mongocfg3.log --logappend

#mongocfg1 
sleep 20

echo 'rs.initiate({_id: "mongors1conf",configsvr: true, members: [{ _id : 0, host : "'${HOSTNAME}':27014" },{ _id : 1, host : "'${HOSTNAME}':27015" }, { _id : 2, host : "'${HOSTNAME}':27016" }]})' | mongosh --port 27014 --quiet

mongos --configdb mongors1conf/${HOSTNAME}:27014,${HOSTNAME}:27015,${HOSTNAME}:27016 --port 27017 --bind_ip_all --fork --logpath=/home/$USER/mongo.log --logappend










