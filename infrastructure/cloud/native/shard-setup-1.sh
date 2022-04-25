USER=$1
HOST1=$2
HOST2=$3
REPL=$4
su $USER
sleep 30
echo 'rs.initiate({_id : "'${REPL}'", members: [{ _id : 0, host : "'$HOST1'" },{ _id : 1, host : "'$HOST2'" }]})' | mongosh --port 27018 --quiet
echo 'rs.status()' | mongosh --port 27018 --quiet
#echo 'db.getMongo().setReadPref("primaryPreferred")' | mongosh --host $HOST2 --port 27018 --quiet
# Alternatively rs.initiate() -> rs.add(HOST:PORT)