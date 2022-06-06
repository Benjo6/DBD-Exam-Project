USER=$1
PASSWORD=$2

DB_USER=$3
DB_PASSWORD=$4

HOSTNAME=$5


su $USER
HOME='/home/'$USER

echo $USER >> ${HOME}/deploy.log
echo $PASSWORD >> ${HOME}/deploy.log
echo $DB_USER >> ${HOME}/deploy.log
echo $DB_PASSWORD >> ${HOME}/deploy.log
echo $HOSTNAME >> ${HOME}/deploy.log

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
cd /repo
git checkout release/exam-dsg

echo "Creating swap" >> ${HOME}/deploy.log
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

sudo chown -R mongodb /home/$USER/data 
sudo chgrp -R mongodb /home/$USER/data 
sudo usermod -aG mongo $USER

echo "Starting Mongo" >> ${HOME}/deploy.log
## Start Mongo ##
sudo sed 's/<USER>/'$USER'/' /repo/infrastructure/cloud/native/mongo/config1.cfg -> /etc/mongocfg1.conf
sudo sed 's/<USER>/'$USER'/' /repo/infrastructure/cloud/native/mongo/config2.cfg -> /etc/mongocfg2.conf
sudo sed 's/<USER>/'$USER'/' /repo/infrastructure/cloud/native/mongo/config3.cfg -> /etc/mongocfg3.conf

sudo cp /repo/infrastructure/cloud/native/mongo/mongocfg1.service /lib/systemd/system/mongocfg1d.service
sudo cp /repo/infrastructure/cloud/native/mongo/mongocfg2.service /lib/systemd/system/mongocfg2d.service
sudo cp /repo/infrastructure/cloud/native/mongo/mongocfg3.service /lib/systemd/system/mongocfg3d.service

sudo systemctl daemon-reload

sudo systemctl start mongocfg1d.service
sudo systemctl start mongocfg2d.service
sudo systemctl start mongocfg3d.service

#mongocfg1 
sleep 20

echo 'rs.initiate({_id: "mongors1conf",configsvr: true, members: [{ _id : 0, host : "'${HOSTNAME}':27014" },{ _id : 1, host : "'${HOSTNAME}':27015" }, { _id : 2, host : "'${HOSTNAME}':27016" }]})' | mongosh --port 27014 --quiet

#Start mongos

sudo sed -e 's/<USER>/'$USER'/' -e 's/<HOSTNAME>/'$HOSTNAME'/' /repo/infrastructure/cloud/native/mongo/mongos.cfg -> /etc/mongos.conf
sudo cp /repo/infrastructure/cloud/native/mongo/mongos.service /lib/systemd/system/mongosd.service
sudo systemctl start mongosd.service

