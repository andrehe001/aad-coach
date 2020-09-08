# GameDay Backend - GameDay Runner API

* Blackbox, team has **NO** access to source code
* Deployed into the trainer environment


## Prepare
1. Create a CosmosDb Mongo API 3.6 instance
1. Take a note of the connectionstring and replace the CONNECTIONSTRING_PLACEHOLDER in appsettings.json
1. Append "&retrywrites=false" at the end of the connectionstring since retrywrites is not supported
1. Create a DB and a collection and replace the DB_NAME and COLLECTION_NAME placeholders in appsettings.json   

## Run

Call this service via HTTP-POST to e.g. https://localhost:5002/GameDayRunner/Start
```
POST http://localhost:5002/GameDayRunner/Start HTTP/1.1
content-type: application/json

{
    "numberOfRequestExecutorsPerTeam": 2,
    "RequestExecutorLatencyMillis": 500,
    "status": "ACTIVE",
    "gameEngineURis": ["https://localhost:5001/Match/"]
}
```

The actual GameDay Runner is a console app that continuously looks up this information.

TODOS: 
* Implement PAUSE, RESUME, EDIT, STOP