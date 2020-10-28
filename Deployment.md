# Deployment Instructions


## Team environment

A Team environment consists of the following azure resources:
- Azure Resource group
- Azure Virtual Network
- Azure Container Registry
- Azure Kubernetes Service
- Azure Application Insights
- Azure Application Gateway V2
- Azure Log Analytics Workspace
- Azure KeyVault

Each team environment is designed to be deployed in different subscriptions.
To automate the deployment of multiple environments use the following script.

### Create just one team deployment

A team can be deployed by running the `deploy-team.sh` script with the following parameters:

```
cd terraform

TEAM_NAME=myteam34 # required, make sure the name dns compatible - lowercase, no special characters, only letters and numbers
TERRAFORM_PATH=terraform # required, full path to terraform executable
LOCATION=westeurope # optional, azure region - if not provided westeurope will be selected
SUBSCRIPTION_ID=$(az account show --query id -o tsv) # optional, if not provided default azure subscription will be selected

./deploy-team.sh $TEAM_NAME $TERRAFORM_PATH $LOCATION $SUBSCRIPTION_ID
```


### Create a set of team environments via csv file

A csv list has to contain at least the team name - which needs to be dns friendly and an azure region name.
Optionally a subscriptionid can be provided - can be left by just adding to ','
The csv file can look like this:
```
team123,westeurope,123456-24654564-564654
team456,northeurope,
```

A set of teams can be deployed by running the `deploy-teams.sh` script with the following parameters:
```
cd terraform

CSV_LIST=teams.csv # required, make sure the name dns compatible - lowercase, no special characters, only letters and numbers
TERRAFORM_PATH=terraform # required, full path to terraform executable

./deploy-teams.sh $CSV_LIST $TERRAFORM_PATH
```

## Deployment team environment using github actions

Deploy using github actions requires a service principal configured in a github actions secret
https://github.com/marketplace/actions/azure-login

```
SUBSCRIPTION_ID=$(az account show --query id -o tsv)
TENANT_ID=$(az account show --query tenantId -o tsv)
RESOURCE_GROUP="aadrg"
LOCATION="northeurope"
TEAM_NAME="teamde234"
TERRA_STATE_RESSOURCE_GROUP="state${TEAM_NAME}${LOCATION}"

az group create -n $RESOURCE_GROUP -l $LOCATION

az ad sp create-for-rbac --name "aaday${RESOURCE_GROUP}" --sdk-auth --role contributor \
    --scopes /subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP

CLIENT_ID=$(az ad sp list --show-mine --display-name "aaday${RESOURCE_GROUP}" --query "[0].appId" -o tsv)

az group create -n $TERRA_STATE_RESSOURCE_GROUP -l $LOCATION

az role assignment create --role "Contributor" --assignee $CLIENT_ID -g $TERRA_STATE_RESSOURCE_GROUP

```

add the json output to the secrets of your own name