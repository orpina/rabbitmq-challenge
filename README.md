# OVERVIEW
Example for publish and subscribe APIs using RabbitMQ as message broker and MediatR to handle events. 

**RabbitMQChallenge.Tracking.API** Publish a LocationUpdateEvent message to two queues: '*analytics-queue*' and '*mapping-queue*'  
**RabbitMQChallenge.Mappging.API** Subscribes to '*mapping-queue*' and adds the processed message into a static list
**RabbitMQChallenge.Analytics.API** Subscribes to '*analytics-queue*' and adds the processed message into a static list
# PREREQUISITES
- Microsoft Visual Studio installed. Used in this example: Community 2022 (64-bit) - Version 17.9.2
- .Net 8.0. Used in this example: SDK 8.0.201
- Docker desktop installed. Used in this example: Docker Desktop 4.28.0 (139021)
- Docker desktop set to Linux containers
- Docker engine up and running
- Ensure you do not have existing containers with name: 
   - >rabbitmq-challenge
   - >rabbitmq-challenge_broker-server
   - >rabbitmq-challenge_mapping-api
   - >rabbitmq-challenge_tracking-api
   - >rabbitmq-challenge_analytics-api
- Ensure you have these ports available:
   - >**5672**  rabbitmq default
   - >**15672** rabbitmq management UI
   - >**3501**  mapping.api
   - >**3502**  tracking.api
   - >**3503**  analytics.api
- **If you already have containers with any of the previous names or any of the previous ports are in use, you will need to update the docker-compose.yaml to rename the container names and/or switch services ports**
# STEPS
- 1 Clone this repository to your PC, use master branch
- 2 Open cloned repository with Visual Studio, switch solution configuration to Release - Any CPU
- 3 Build Solution
- 4 Run command: docker compose build
- 5 Run command: docker compose up
- 6 Wait until all containers are successfully created and initiallized. **If any of the service containers inside of the rabbitmq-challenge container is paused or stopped you will need to start them manually**
# RABBITMQ CONTAINER SERVICE
Container created from the image  '*3.13.0-management*'  available from the oficial images in https://hub.docker.com/_/rabbitmq  
**RabbitMQ images are only available for Linux**
### URL
 http://localhost:15672/#/
### Credentials
**User**:
 -     rabbitmqchallengeuser  
**Password**:
 -     rabbitmqchallengepass  
**Host Name**:
 -     rabbitserver117
# API'S
## RabbitMQChallenge.Mapping.API
### Swagger Access:
- https://localhost:3501/swagger/index.html
### GeoPoints
- Get GeoPoints from location updates received
  - Action Method: GET, Replace device_id with desired id
    - https://localhost:3501/api/GeoPoints/GetById/device_id
## RabbitMQChallenge.Analytics.API
### Swagger Access:
- https://localhost:3503/swagger/index.html
### Analytics
- Get Device with its location updates
  - Action Method: GET, Replace device_id with desired id
    - https://localhost:3503/api/Analytics/GetById/device_id 
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
# TESTING WORKFLOW
Once all containers are up and running
- Send a location update using the POST method in **RabbitMQChallenge.Tracking.API**. You can use the Swagger Access of the API and the payload example or the GPS SIMULATOR.
- After succesfully completing the previous step you can access the subscriber API's to validate if the message was received.
  	- **RabbitMQChallenge.Analytics.API**. Access to the exposed GET method, you can access the GET endpoint URL or through the Swagger Access. You will need to replace the *device_id* for the value used in the POST (*A104-B1500023-0001* if you used the payload example)
  	- **RabbitMQChallenge.Mapping.API**. Access to the exposed GET method, you can access the GET endpoint URL or through the Swagger Access. You will need to replace the *device_id* for the value used in the POST (*A104-B1500023-0001* if you used the payload example)
- In both subscriber API's you should be able to see the result in the response after triggering the GET requests.
# GPS SIMULATOR
 Small console app to trigger location update requests to the RabbitMQChallenge.Tracking.API
 - Variables
 	- **useContainerAPI**: set to true to trigger with the the *containerizedTrackingAPIURL*. If false you will need to set the url in the *localTrackingAPIURL* variable
	- **containerizedTrackingAPIURL**: URL of the containerized tracking API
	- **localTrackingAPIURL**: URL of the tracking API when debugging
 	- **totalRequests**: is the number of requests to be triggered
# LOCAL DEBUG
In order to debug project API's instead of launch the docker containers
- Switch solution configuration to Debug - Any CPU
- Build solution
- In the appsettings.Development.json of the API you want to debug update **UseLocalService** from false to true. This will create a connection using LocalHostName, LocalUserName and LocalPassword configs.
- Right click on API project and click on Debug > Start New Instance
  
**Notes**:
- You will need to install the RabbitMQ server into your local machine https://www.rabbitmq.com/docs/install-windows#installer
- If you are not able to access the RabbitMQ Management UI (port 15672) you may need to run this command:
	-     rabbitmq-plugins enable rabbitmq_management
-  Local configurations in appsettings.Development.json for RabbitMQ connection are set to the default values when running local server. LocalHostName = localhost, LocalUserName = guest, LocalPassword = guest. You can create a new host, user, password through the Management UI or the console and update and then update this values in the appsettings.Development.json

