# Challenge 1

```
apiVersion: v1
kind: Service
metadata:
  name: blackboxgameengine
  labels:
    name: blackboxgameengine    
spec:
  selector:
    name: blackboxgameengine    
  type: LoadBalancer
  externalTrafficPolicy: Local
  ports:
    - port: 80
      name: blackboxgameengine
      targetPort: 80
      protocol: TCP
    - port: 81
      name: istio-proxy
      targetPort: 81
      protocol: TCP    
---
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: default-reader
  namespace: default
subjects:
- kind: Group
  name: system:serviceaccounts
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: cluster-admin
  apiGroup: rbac.authorization.k8s.io
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: blackboxgameengine
spec:
  replicas: 1
  selector:
    matchLabels:
      name: blackboxgameengine      
  minReadySeconds: 5
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 1
  template:
    metadata:
      labels:
        name: blackboxgameengine        
    spec:
      containers:
      - name: blackboxgameengine
        image: ghcr.io/azure-adventure-day/azure-adventure-day-coach/gamedayengine-web:latest
        imagePullPolicy: Always
        ports:
          - containerPort: 80
            name: gameengine
            protocol: TCP
        env: 
          - name: "ConnectionStrings__GameEngineDB"
            value: "Server=tcp:gamesqlserver7qeg9.database.windows.net,1433;Initial Catalog=gamedb;Persist Security Info=False;User ID=gamedbadministrator;Password=mJU}}$%1zeEEG5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
          - name: "ARCADE_BACKENDURL"
            value: "http://gamebot/pick"
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "200m"
        livenessProbe:
          httpGet:
            path: /Match
            port: 80
        readinessProbe:
          httpGet:
            path: /Match
            port: 80
      - name: istio-proxy
        image: ghcr.io/azure-adventure-day/azure-adventure-day-coach/gamedayengine-sidecar:latest
        imagePullPolicy: Always
        volumeMounts:
        - mountPath: /meshconfig
          name: config-volume
        ports:
          - containerPort: 81
            name: servicemesh         
            protocol: TCP
        env: 
          - name: "ASPNETCORE_URLS"
            value: "http://*:81"
      volumes:
      - name: config-volume
        hostPath:
          path: /etc/kubernetes/



apiVersion: v1
kind: Service
metadata:
  name: gamebot
  labels:
    name: gamebot    
spec:
  selector:
    name: gamebot    
  type: LoadBalancer
  ports:
   - port: 80
     name: gamebot
     targetPort: 8080
     protocol: TCP
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: gamebot
spec:
  replicas: 3
  selector:
    matchLabels:
      name: gamebot      
  minReadySeconds: 10
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxUnavailable: 1
      maxSurge: 1 
  template:
    metadata:
      labels:
        name: gamebot        
    spec:
      imagePullSecrets:
        - name: teamregistry
      containers:
      - name: gamebot
        image: team09acr.azurecr.io/gamebot:highrisk
        imagePullPolicy: Always
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
        livenessProbe:
            httpGet:
              path: /health
              port: 8080
        readinessProbe:
            httpGet:
              path: /health
              port: 8080
        ports:
          - containerPort: 8080     
            protocol: TCP
            name: gamebot
        env: 
          - name: "PORT"
            value: "8080"
          - name: "PICK_STRATEGY"
            value: "RANDOM"
          - name: "FF_BETS"
            value: "true"

```

# Challenge 2 Build containers

```
REGISTRY_URL=team09acr.azurecr.io
REGISTRY_NAME=team09acr
REGISTRY_PASSWORD=z1txwqD8UAD5A/MSrPN7yPJARXhoY6IZ
kubectl create secret docker-registry teamregistry --docker-server $REGISTRY_URL --docker-username $REGISTRY_NAME --docker-password $REGISTRY_PASSWORD --docker-email 'example@example.com'
az configure --defaults acr=$REGISTRY_NAME
az acr build --image=gamebot .

KUBE_GROUP=team09
KUBE_NAME=team09

kubectl rollout restart deployment/blackboxgameengine

kubectl rollout restart deployment/gamebot

kubectl logs -l name=blackboxgameengine -c blackboxgameengine
```

## Add Node Pools
```
az aks nodepool list --cluster-name $KUBE_NAME -g $KUBE_GROUP -o table

az aks nodepool add --enable-cluster-autoscaler --node-count=2 --min-count 1 --max-count 5 -n gameworker --cluster-name $KUBE_NAME -g $KUBE_GROUP --node-vm-size=Standard_B2ms --mode User

az aks nodepool delete -n gameworker --cluster-name $KUBE_NAME -g $KUBE_GROUP

az aks nodepool delete -n default --cluster-name $KUBE_NAME -g $KUBE_GROUP

kubectl top pods --all-namespaces
```

## Configure autoscaler
```
kubectl autoscale deploy blackboxgameengine --cpu-percent=20 --max=10 --min=1

kubectl autoscale deploy gamebot --cpu-percent=20 --max=30 --min=1


kubectl delete hpa blackboxgameengine

apiVersion: autoscaling/v2beta2
kind: HorizontalPodAutoscaler
metadata:
  name: blackboxgameengine
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: blackboxgameengine
  minReplicas: 1
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 40
  - type: Resource
    resource:
      name: memory
      target:
        type: AverageValue
        averageValue: 90Mi
```

## Configure Minimal rolebindings

```
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  name: default-nodes-get
rules:
- apiGroups: [""]
  resources: ["nodes"]
  verbs: ["get", "list"]
---
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: default-reader
  namespace: default
subjects:
- kind: Group
  name: system:serviceaccounts
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: default-nodes-get
  apiGroup: rbac.authorization.k8s.io
```


## Application Insights

```
customEvents |
where name == "match" |
where customDimensions contains "Courtney" |
order by timestamp |
project customDimensions.move, 1 |
summarize count(), by tostring(customDimensions_move)
```
```
