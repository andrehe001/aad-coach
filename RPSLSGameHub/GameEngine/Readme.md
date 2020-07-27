# RPSLS Game Hub - Game Engine

* Blackbox, team has **NO** access to source code
* Deployed into each team environment


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
1. Call this service via HTTP-POST to e.g. https://localhost:32770/Match/.
1. POST this content:
	```
	{
    "ChallengerId": "daniel",    
    "Move":"Paper"    
	}
	```
1. For subsequent calls provide the matchId found in the response.


## How it works
1. The challenger calls the game engine with the first turn. 
1. The game engine creates a "match" and queries the backend service for it's turn.
1. The game engine saves the state of this "match" to redis.
1. The game engine returns the outcome of this move to the challenger.
1. The challenger can make the second move and so on.
1. After the match, all moves will be store in the DB including results.
