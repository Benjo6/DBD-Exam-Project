cp /scripts/setup_conf.js /docker-entrypoint-initdb.d/
mongod --port 27017 --configsvr --replSet rs-config-server --bind_ip_all