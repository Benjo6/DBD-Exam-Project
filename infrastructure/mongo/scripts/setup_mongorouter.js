sh.addShard("rs-sh-01/mongo-shard1-rs1:27017");
sh.addShard("rs-sh-01/mongo-shard1-rs2:27017");
sh.addShard("rs-sh-01/mongo-shard1-rs3:27017");
sh.addShard("rs-sh-02/mongo-shard2-rs1:27017");
sh.addShard("rs-sh-02/mongo-shard2-rs2:27017");
sh.addShard("rs-sh-02/mongo-shard2-rs3:27017");

const db = new Mongo().getDB('consultations');
sh.enableSharding("consultations");
sh.shardCollection( "consultations.consultations", { "_id" : "hashed" } )

db.consultations.createIndex( { Location : "2dsphere" } )