const db = new Mongo().getDB('consultations');
db.consultations.createIndex( { Location : "2dsphere" } )