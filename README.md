# DBD-Exam-Project
Very interesting assignment



# Connectionstrings for dev:
All require running .start-dev.bat from root of repository (shortcut for docker-compose up with the databases along with services that have been containerized)
This will set up databases and services on the following ports:

| Service  | Local Development | From other docker containers |
|----------|-------------------|------------------------------|
| Postgres                | localhost:15432   | postgres:5432           |
| Mongo                   | localhost:17017   | mongo:27017             |
| Neo4j                   | localhost:17474   | neo:7474              |
| Neo4j                   | localhost:17687   | neo:7687              |
| Redis                   | localhost:16379   | redis:6379              |
| Consultation Service    | localhost:18090   | consultation-service:80 |
| Mail Service            | localhost:18092   | mail-service:80         |
| Prescription Service    | localhost:18093   | prescription-service:80 |
| Test Data Api           | localhost:18094   | testdata-service:80     |
| CronJob Service         | localhost:18095   | cronjob-service:80      |
| Data Analyzing Service  | localhost:18096   | analyzing-service:80    |
| Blazor Frontend         | localhost:8080    | blazor-frontend:80 |

| Tool | Url |
| - | - |
| PG Admin | http://localhost:10010
| Mongo Express | http://localhost:10020
| Redis Insight | http://localhost:10030

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

