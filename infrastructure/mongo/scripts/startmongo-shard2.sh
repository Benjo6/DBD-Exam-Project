cp /scripts/setup_shard2.js /docker-entrypoint-initdb.d/
mongod --port 27017 --shardsvr --replSet rs-sh-02 --bind_ip_all