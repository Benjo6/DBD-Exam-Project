USER=$1
PASSWORD=$2
DB_USER=$3
DB_PASSWORD=$4
HOST1=$5
HOST2=$6
REPL=$7
su $USER
sleep 10



echo 'rs.initiate({_id : "'${REPL}'", members: [{ _id : 0, host : "'$HOST1'" },{ _id : 1, host : "'$HOST2'" }]})' | mongosh --port 27018 --quiet
echo 'rs.status()' | mongosh --port 27018 --quiet

sudo systemctl stop mongoshardd.service
sudo rm /etc/mongod.conf
sudo curl https://raw.githubusercontent.com/SOFT2022-Database-Exam/DBD-Exam-Project/release/exam-dsg/infrastructure/cloud/native/mongo/shardauth.cfg -o /etc/mongodauthbase.conf
sudo sed -e 's/<USER>/'$USER'/' -e 's/<REPL>/'$REPL'/' /etc/mongodauthbase.conf -> /etc/mongod.conf
sudo systemctl start mongoshardd.service
