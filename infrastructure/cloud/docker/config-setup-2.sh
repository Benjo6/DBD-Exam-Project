echo "config setup 1"
docker exec mongos1 bash -c "echo 'sh.addShard(\"mongors1/mongo-shard1-1:27017\")' | mongosh --quiet" 
docker exec mongos1 bash -c "echo 'sh.addShard(\"mongors2/mongo-shard2-1:27017\")' | mongosh --quiet" 

docker exec -it mongos1 bash -c "echo 'sh.status()' | mongosh --quiet" 

echo "**Setup Shards**\n"
docker exec mongos1 bash -c "echo 'sh.enableSharding(\"twitter\")' | mongosh --quiet"
docker exec mongos1 bash -c "echo 'sh.shardCollection(\"twitter.tweets\", {\"source\" : \"hashed\"})' | mongosh --quiet"


echo "**Import data**\n"
docker exec mongos1 bash -c "mongoimport --db twitter --collection tweets --type json /docker/data/twitter.json"