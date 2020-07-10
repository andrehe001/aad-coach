#!/usr/bin/env bash
set -o pipefail

# ./deploy-team.sh team1 . westeurope subscriptionid


export team_name="$1"
export terra_path="$2"
export location="$3"
export subscriptionid="$4"

TERRA_PATH=$terra_path/terraform

if [ "$location" == "" ]; then
location="westeurope"
echo "No location provided - defaulting to $location"
fi

if [ "$subscriptionid" == "" ]; then
subscriptionid=$(az account show --query id -o tsv)
echo "No subscriptionid provided defaulting to $subscriptionid"
else
az account set --subscription $subscriptionid
fi

tenantid=$(az account show --query tenantId -o tsv)

echo "This script will create an environment for team $team_name in $location"

TERRAFORM_STORAGE_NAME="t$team_name$location"
TERRAFORM_STATE_RESOURCE_GROUP_NAME="state$team_name$location"

echo "creating terraform state storage..."
TFGROUPEXISTS=$(az group show --name $TERRAFORM_STATE_RESOURCE_GROUP_NAME --query name -o tsv --only-show-errors)
if [ "$TFGROUPEXISTS" == $TERRAFORM_STATE_RESOURCE_GROUP_NAME ]; then 
echo "terraform storage resource group $TERRAFORM_STATE_RESOURCE_GROUP_NAME exists"
else
echo "creating terraform storage resource group $TERRAFORM_STATE_RESOURCE_GROUP_NAME..."
az group create -n $TERRAFORM_STATE_RESOURCE_GROUP_NAME -l $location --output none
fi

TFSTORAGEEXISTS=$(az storage account show -g $TERRAFORM_STATE_RESOURCE_GROUP_NAME -n $TERRAFORM_STORAGE_NAME --query name -o tsv)
if [ "$TFSTORAGEEXISTS" == $TERRAFORM_STORAGE_NAME ]; then 
echo "terraform storage account $TERRAFORM_STORAGE_NAME exists"
TERRAFORM_STORAGE_KEY=$(az storage account keys list --account-name $TERRAFORM_STORAGE_NAME --resource-group $TERRAFORM_STATE_RESOURCE_GROUP_NAME --query "[0].value" -o tsv)
else
echo "creating terraform storage account $TERRAFORM_STORAGE_NAME..."
az storage account create --resource-group $TERRAFORM_STATE_RESOURCE_GROUP_NAME --name $TERRAFORM_STORAGE_NAME --location $location --sku Standard_LRS --output none
TERRAFORM_STORAGE_KEY=$(az storage account keys list --account-name $TERRAFORM_STORAGE_NAME --resource-group $TERRAFORM_STATE_RESOURCE_GROUP_NAME --query "[0].value" -o tsv)
az storage container create -n tfstate --account-name $TERRAFORM_STORAGE_NAME --account-key $TERRAFORM_STORAGE_KEY --output none
fi

echo "initialzing terraform state storage..."

$TERRA_PATH init -backend-config="storage_account_name=$TERRAFORM_STORAGE_NAME" -backend-config="container_name=tfstate" -backend-config="access_key=$TERRAFORM_STORAGE_KEY" -backend-config="key=codelab.microsoft.tfstate" ./team

echo "planning terraform..."
$TERRA_PATH plan -out $team_name-out.plan -var="deployment_name=$team_name" -var="location=$location" -var="tenant_id=$tenantid" -var="subscription_id=$subscriptionid"  ./team

echo "running terraform apply..."
$TERRA_PATH apply $team_name-out.plan
