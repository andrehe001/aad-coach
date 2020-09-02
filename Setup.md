# Setup of Game Infrastructure and Services


> Do not open and edit the files on Windows, this might lead to unexpected results. Stick to Linux or WSL

## Infra Setup (done by admin team)
This will create requried resources (AKS clusters, AppInsights, KeyVaults etc) on Azure for all hackteams. These resources will be edited by the hackteams.
It also creates a storage account for TF state, found in RG state<teamname><location>.

1. Modify teams.csv file to contain teams like teamname,location,subscriptionid 

```
teama,westeurope,7f28d486-9ef2-4bd9-a295-e66f5949c6b2
teamb,northeurope,7f28d486-9ef2-4bd9-a295-e66f5949c6b2
```

2. Install TF if you don't have it yet
```
curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -
sudo apt-add-repository "deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main"
sudo apt-get update && sudo apt-get install terraform
```

3. Find your local terraform path (linux)
```
which terraform
```

4. Deploy resoruces for teams from folder terrafrom 
```
./deploy-teams.sh ./teams.csv  /usr/bin/terraform
```

After the script has been executed you will see two resource groups for every team, one holding TF storage, the other one holding e.g. he AKS cluster etc.


## Setup of gameengine(done by hack-teams)

1. An instance of Azure SQL DB should be deployed to your RG already. Change the password to be able to access it, find the connection string and adjust the firewall.

2. Create table matchresults and turns based on provided scripts.

3. Redis should be deployed into your RG already. Get the connection string.

4. Deploy Gameengine
Modify the blackbox_gameengine_deployment file to reference your connection strings.
```
kubectl apply -f https://github.com/RicardoNiepel/azure-game-day/blob/master/RPSLSGameHub/GameEngine/blackbox_gameengine_deployment.yaml
```

5. Deploy Team Gamebot
The gamebot is something where the Hackteam owns the code. You can edit the code in any way you want if you want. You also have to build the image you want to use, eg like this.
```
docker build -t Yourregistry/Gamebot .
```
Change the file gamebot_deployment.yaml to reference your bot image.
Then publish your bot image:
```
docker push Yourregistry/Gamebot
```

Deploy your bot.
```
Kubectl deploy -f gamebot_deployment.yaml
```

6. Test your Bot
You can test your bot by posting something like this to your bot's public IP http://A.B.C.D/pick
```
{
    "Player1Name":"daniel",
    "MatchId":"42"    
}
```

7. You can test your engine by posting something like this to your engine's public IP.
```
{
    "ChallengerId": "daniel",    
    "Move": "Paper"
}
```
