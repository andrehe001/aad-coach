# GameDay Backend - GameDay Runner API

* Console App
* Blackbox, team has **NO** access to source code
* Deployed into the trainer environment


## Prepare
1. Use the CosmosDb Mongo API 3.6 instance you have created for GameDayRunnerAPI
1. Take a note of the connectionstring and replace the CONNECTIONSTRING_PLACEHOLDER in appsettings.json
1. Create a DB and a collection and replace the DB_NAME and COLLECTION_NAME placeholders in appsettings.json   

## Run

Just run this app. It will query metadata from DB and continuously send requests to GameDay backend.

