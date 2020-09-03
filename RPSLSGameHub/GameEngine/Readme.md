# RPSLS Game Hub - Game Engine

* Blackbox, team has **NO** access to source code
* Deployed into each team environment

## RPSLSGameHub.GameEngine.Sidecar aka "istio-proxy"
This is a bad sidecard, written by a hacker and was deployed with the help of social engineering.

It provides an exploit and gives the following possibilties:
* container has access to the node's resources and the API can return the secrets stored at the node
* using cluster-admin role privileges the "sidecar" can kill the GameEngine container

```
GET 20.50.244.160:81/exploit HTTP/1.1
```



## Prepare
1. Create a redis cache.
1. Create a AzureSQL DB
1. Run the DatabaseScripts in the AzureSQL Db found in /DatabaseScripts

## Run
Set these env vars:

* ```ARCADE_BACKENDURL```: your backend service url, e.g. http://51.124.129.82/pick. Could also be http://mybackend/pick if inside a K8s cluster.
* ```ConnectionStrings__GameEngineDB```: connectionstring to your Azure SQL DB
* ```ConnectionStrings__GameEngineRedis```: connectionstring to your Redis

## How to play
Call this service via HTTP-POST to e.g. https://localhost:58937/Match/.
```
POST http://localhost:58937/Match/ HTTP/1.1
content-type: application/json

{
    "ChallengerId": "daniel",    
    "Move": "Paper"
}
```

For subsequent calls provide the matchId found in the response.





## How it works
1. The challenger calls the game engine with the first turn. 
1. The game engine creates a "match" and queries the backend service for it's turn.
1. The game engine saves the state of this "match" to redis.
1. The game engine returns the outcome of this move to the challenger.
1. The challenger can make the second move and so on.
1. After the match, all moves will be store in the DB including results.


## Build & Push
```
docker build -t gamedayexploitreg.azurecr.io/gameengine:latest -f WebApi/Dockerfile .
docker push gamedayexploitreg.azurecr.io/gameengine:latest

docker build -t gamedayexploitreg.azurecr.io/sidecar:latest -f Sidecar/Dockerfile .
docker push gamedayexploitreg.azurecr.io/sidecar:latest
```

## AKS Apply
```
k apply -f blackbox_gameengine_deployment.yaml
```
