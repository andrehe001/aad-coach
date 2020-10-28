#!/usr/bin/env bash
set -o errexit
set -o pipefail
set -o nounset

# ./import-teams.sh csvfile.csv apirul

export file="$1"
export adminserviceprincipalid="$2"
export adminserviceprincipalsecret="$3"


OLDIFS=$IFS
IFS=';'
[ ! -f $file ] && { echo "$file file not found"; exit 99; }
while read teamname subscriptionid tenantid teampassword comment
do
    echo "switching into subscription for $teamname to subscription $subscriptionid in tenant $tenantid..."
    #az login --service-principal --username $adminserviceprincipalid --password $adminserviceprincipalsecret --tenant c2630420-378e-4563-95a4-519774788b5b
    az login -u $adminserviceprincipalid -p $adminserviceprincipalsecret --tenant $tenantid
    echo "setting subscription to $subscriptionid"
    az account set --subscription $subscriptionid
    echo "registering azure security resource provider"
    az provider register --namespace Microsoft.Security
    echo "activating azure security center"
    az security pricing create -n KubernetesService --tier 'standard'
    
done < $file
IFS=$OLDIFS