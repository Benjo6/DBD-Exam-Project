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
| Consultation Service    | localhost:18090   |              --              |
| Analysis Service        | localhost:18091   |              --              |
| Mail Service            | localhost:18092   |              --              |
| Prescription Service    | localhost:18093   |              --              |
| Test Data Api           | localhost:18094   |              --              |
| CronJob Service         | localhost:18095   |              --              |

neo4j
## ConsultationService (mongo)
Service for creating, booking and retrieving consultations.

When running through docker the service is accessed at http://localhost:18090/

### Example of geolocation

![Creation](/documentation/mongo/consultationcreate.png)
![Creation Response](/documentation/mongo/consultationcreate_response.png)

When getting bookings available near you, you pass long/lat and distance in meters as path params.
10.000m will be slightly less than 0.09 latitude, as seen here:

![Success](/documentation/mongo/withinrange.png)
While >= 0.09 difference will not return a result: 

![No Result](/documentation/mongo/outsiderange.png)

## postgres
### docker
```"Host=prescription-database;Port=5432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw"```
### localhost
```"Host=localhost;Port=15432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw"```
redis
