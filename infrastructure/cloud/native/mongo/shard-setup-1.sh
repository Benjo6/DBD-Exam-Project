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

echo 'new Mongo("localhost:27018").getDB("admin").createUser({ user: "'${USER}'",pwd: "'${PASSWORD}'", roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]})' | mongosh --port 27017 --quiet
echo 'new Mongo("localhost:27018").getDB("admin").createUser( { user: "'${DB_USER}'", pwd: "'${DB_PASSWORD}'", roles: [ { role: "readWrite", db: "consultations" } ] })' | mongosh --port 27017 --quiet


sudo systemctl stop mongosd.service


sudo systemctl start mongosd.service
