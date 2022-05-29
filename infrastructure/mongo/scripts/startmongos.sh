cp /scripts/setup.js /docker-entrypoint-initdb.d/
mongos --port 27017 --configdb rs-config-server/mongo-conf1:27017,mongo-conf2:27017,mongo-conf3:27017 --bind_ip_all
cat 