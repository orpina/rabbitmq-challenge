# PREREQUISITES:
- Docker desktop installed
- Docker desktop set to Linux containers
- Docker engine up and running
- Ensure you do not have existing containers with name: 
   - >rabbitmq-challenge
   - >rabbitmq-challenge_broker-server
   - >rabbitmq-challenge_mapping-api
   - >rabbitmq-challenge_tracking-api
- Ensure you have these ports available:
   - >5672  rabbitmq
   - >15672 rabbitmq
   - >3501  mapping.api
   - >3502  tracking.api
- ### If you already have containers with any of the previous names or any of the previous ports are in use, you will need to update the docker-compose.yaml to rename the container names and/or switch services ports

# STEPS:
- 1 Switch solution configuration to Release - Any CPU
- 2 Build Solution
- 3 Run command: docker compose build
- 4 Run command: docker compose up

Wait until all containers are successfully created and initiallized.If any of the following service containers inside of the rabbitmq-challenge container is paused or stopped you will need to start them manually: rabbitmq-challenge_mapping-api, rabbitmq-challenge_tracking-api

# API'S
## RabbitMQChallenge.Mapping.API
### Swagger Access:
- https://localhost:3501/swagger/index.html
### GeoPoints
- Get GeoPoints from location updates received
  - Action Method: GET, Replace device_id with desired id
    - https://localhost:3501/api/GeoPoints/GetById/device_id 
## RabbitMQChallenge.Tracking.API
### Swagger Access:
- https://localhost:3502/swagger/index.html
### Location
- Add new location update
  - Action Method: POST
    - https://localhost:3502/api/Location
          
		Payload example:
      -     {
              "deviceId": "A104-B1500023-0001",
              "latitude": 120.10093,
              "longitude": 78.45061
            }
