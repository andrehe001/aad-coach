# Quick Setup for Team enviornments (GameEngine & Bot)
Log into Cloudshell for team and run these lines there. Be aware there are manual steps after terraform deployment.

```
## Set Subscription & Team

## Replace with correct values
SUBSCRIPTION_ID=6bd73137-0103-4141-bf0b-0734f8a5be22
TEAM=team12

git clone https://github.com/azure-adventure-day/aad-team.git
cd aad-team
cd terraform/
chmod +x ./deploy-team.sh
./deploy-team.sh team12 northeurope $SUBSCRIPTION_ID


## Manual Steps
##replace DB Connection string in blackbox_gameengine_deployment.yaml
## set firewall in Azure SQL
## and create tables in Azure SQL DB

cd ..
az aks get-credentials -n $TEAM -g $TEAM
kubectl apply -f GameEngine/blackbox_gameengine_deployment.yaml
kubectl apply -f GameBot/gamebot_deployment.yaml 
GAMEBOT_IP=$(kubectl get services --field-selector metadata.name=arcadebackend --output=jsonpath={.items..status.loadBalancer.ingress..ip})
GAMEENGINE_IP=$(kubectl get services --field-selector metadata.name=blackboxgameengine --output=jsonpath={.items..status.loadBalancer.ingress..ip})
echo "GAMEBOT_IP:" $GAMEBOT_IP
echo "GAMEENGINE_IP:" $GAMEENGINE_IP
curl --location --request POST http://$GAMEENGINE_IP/Match --header 'Content-Type: application/json' --data-raw '{"ChallengerId":"daniel","Move": "Rock"}'
```
