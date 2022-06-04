USER=$1
su $USER

echo "config setup 2"
echo 'sh.addShard("mongors1/vm-a-1:27018")' | mongosh --quiet
echo 'sh.addShard("mongors2/vm-b-1:27018")' | mongosh --quiet

echo "sh.status()" | mongosh --quiet

echo "**Setup Shards**"
const db = new Mongo().getDB('consultations');
sh.enableSharding("consultations");
sh.shardCollection( "consultations.consultations", { "_id" : "hashed" } )
db.consultations.createIndex( { Location : "2dsphere" } )

sh.shardCollection( "consultations.consultations_booked", { "_id" : "hashed" } )
