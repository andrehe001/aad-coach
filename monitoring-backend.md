# Monitoring the Adventure Day Backend

The adventure day backend exports metrics via [Prometheus](https://prometheus.io/) and logs all information to STDOUT. Logs are collected by [Loki](https://grafana.com/oss/loki/).

All information can be accessed via the [Grafana](https://grafana.com/) Dashbaord.

To access the dashboard run the following commands:

```sh
export COACH_ID="02"
export COACH_PASSWORD=""
export COACH_SUBSCRIPTION_ID=""

az login -u coach${COACH_ID}@asmw13.onmicrosoft.com -p "${COACH_PASSWORD}"
az account set -s ${COACH_SUBSCRIPTION_ID}

az aks get-credentials --resource-group azure_adventure_day_prod_rg --name azure-adventure-day-prod --overwrite-existing --admin
kubectl port-forward deployment/po-grafana 3000 3000 -n monitoring
```

Navigate to [http://localhost:3000](http://localhost:3000).

```txt
UserName: admin
Password: prom-operator
```

If this is the initial login to Grafana after the cluster has been created, you need to configure a Loki connector:
```
URL: http://loki:3100
```

