# DBD-Exam-Project
Very interesting assignment



# Connectionstrings for dev:
All require running .start-dev.bat from root of repository (shortcut for docker-compose up with the databases along with services that have been containerized)
This will set up databases and services on the following ports:

| Service  | Local Development | From other docker containers |
|----------|-------------------|------------------------------|
| Postgres                | localhost:15432   | db-exam-postgres:5432        |
| Mongo                   | localhost:17017   | db-exam-mongo:17017          |
| Neo4j                   | localhost:17474   | db-exam-neo4j:17474          |
| Neo4j                   | localhost:17687   | db-exam-neo4j:17687          |
| Redis                   | localhost:16379   | db-exam-redis:16379          |
| Consultation Service    | localhost:18088   |              --              |

neo4j
## mongo
ConnectionString for local development

```db-exam-mongo-config.northeurope.cloudapp.azure.com:17017```
## postgres
### docker
```"Host=prescription-database;Port=5432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw"```
### localhost
```"Host=localhost;Port=15432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw"```
redis
