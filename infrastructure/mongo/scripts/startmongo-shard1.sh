cp /scripts/setup_shard1.js /docker-entrypoint-initdb.d/
mongod --port 27017 --shardsvr --replSet rs-sh-01 --bind_ip_all