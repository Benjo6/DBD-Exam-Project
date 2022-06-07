USER=$1
REPL=$2
KEY=$3

su $USER
sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile && mkswap /swapfile 
sudo swapon /swapfile
sudo echo "\n/swapfile swap swap defaults 0 0" >> /etc/fstab
sudo mkdir /data && chmod 777 /data


## Install Mongo ##
wget -qO - https://www.mongodb.org/static/pgp/server-5.0.asc | sudo apt-key add -
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/5.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-5.0.list
sudo apt-get update
sudo apt-get install -y mongodb-org=5.0.6 mongodb-org-database=5.0.6 mongodb-org-server=5.0.6 mongodb-org-shell=5.0.6 mongodb-org-mongos=5.0.6 mongodb-org-tools=5.0.6

echo 'new Mongo("localhost:27018").getDB("admin").createUser({ user: "'${USER}'",pwd: "'${PASSWORD}'", roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]})' | mongosh --port 27018 --quiet
echo 'new Mongo("localhost:27018").getDB("admin").createUser( { user: "'${DB_USER}'", pwd: "'${DB_PASSWORD}'", roles: [ { role: "readWrite", db: "consultations" } ] })' | mongosh --port 27018 --quiet

mkdir -p /home/$USER/data/db

echo $KEY >> /home/$USER/data/key.txt
sudo chmod 400 /home/$USER/data/key.txt

sudo chown -R mongodb /home/$USER/data 
sudo chgrp -R mongodb /home/$USER/data 
sudo usermod -aG mongo $USER

sudo curl https://raw.githubusercontent.com/SOFT2022-Database-Exam/DBD-Exam-Project/release/exam-dsg/infrastructure/cloud/native/mongo/shard.cfg -o /etc/mongodbase.conf
sudo curl https://raw.githubusercontent.com/SOFT2022-Database-Exam/DBD-Exam-Project/release/exam-dsg/infrastructure/cloud/native/mongo/mongoshardd.service -o /lib/systemd/system/mongoshardd.service
sudo sed -e 's/<USER>/'$USER'/' -e 's/<REPL>/'$REPL'/' /etc/mongodbase.conf -> /etc/mongod.conf
sudo systemctl daemon-reload
sudo systemctl start mongoshardd.service