## B-Q01 - Denmark transports arrival notification service

Little side-project to track and notify of bus (or other public transports) arriving at a specific stop in Denmark, using the Rejseplanen API.

The system has two background services, one that periodically queries the Rejseplanen API for updates on the next arrival and then writes them to an in-memory database, and another that queries the database and sends and event to a Kafka broker when a bus is arriving under a specified timeframe.

This works as the backend system, and using a RaspberryPi I query the Kafka messages to receive and phisically notify me of an upcoming bus.

### How to run in your machine

1. Get an API key at [Rejseplanen Labs](https://labs.rejseplanen.dk/hc/en-us)
2. In a `appsettings.json` file, add your Kafka settings, Rejseplanen API key, and the stop ID that can be found in Rejseplanen Labs. Currently it's only possible to track a single stop, although multiple stops support may be added in a later date

       {
          "KafkaProducer": {
          "BootstrapServers": "localhost:9092",
          "ClientId": "RejseplanenCollector",
          "Acks": "0"
          },
          "Rejseplanen": {
          "Stops": "Your stop ID",
          "ApiKey": "Your key"
          }
        }
   
4. Run the docker container

        docker compose up --build

**That's it!**
