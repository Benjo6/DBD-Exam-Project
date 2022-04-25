docker exec mongors bash -c "echo 'rs.initiate({_id : \"mongors2\", members: [{ _id : 0, host : \"mongo-shard2-1\" },{ _id : 1, host : \"mongo-shard2-2\" }]})' | mongosh --quiet"
docker exec mongors bash -c "echo 'rs.status()' | mongosh --quiet"
docker exec mongors bash -c "echo 'db.getMongo().setReadPref(\"primaryPreferred\")' | mongosh --quiet"
