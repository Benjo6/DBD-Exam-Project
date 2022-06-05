USER=$1
PASSWORD=$2
DB_USER=$3
DB_PASSWORD=$4
HOSTNAME=$5

su $USER
cd /home/${USER}
echo $PWD

echo "config setup 2"
echo 'sh.addShard("mongors1/vm-a-1:27018")' | mongosh --quiet
echo 'sh.addShard("mongors2/vm-b-1:27018")' | mongosh --quiet


echo "**Setup Shards**" >> deploy.log

echo 'sh.enableSharding("consultations")' | mongosh --quiet
echo 'sh.shardCollection( "consultations.consultations", { "_id" : "hashed" } )' | mongosh --quiet
echo 'new Mongo().getDB("consultations").consultations.createIndex( { Location : "2dsphere" } )' | mongosh --quiet
echo 'sh.shardCollection( "consultations.consultations_booked", { "_id" : "hashed" } )' | mongosh --quiet

echo "Create Mongo Admin User" >> deploy.log
echo 'db.createUser({ user: "'${USER}'",pwd: "'${PASSWORD}'", roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]})' | mongosh --port 27017 --quiet
echo 'db.createUser( { user: "'${DB_USER}'", pwd: "'${DB_PASSWORD}'", roles: [ { role: "readWrite", db: "consultations" } ] })' | mongosh --port 27017 --quiet


# Stop mongos and update config
sudo systemctl stop /lib/systemd/system/mongos.service
sudo rm /etc/mongos.conf
sudo sed -e 's/<USER>/'$USER'/' -e 's/<HOSTNAME>/'$HOSTNAME'/' /repo/infrastructure/cloud/native/mongo/mongosauth.cfg -> /etc/mongos.conf
sudo systemctl start /lib/systemd/system/mongos.service