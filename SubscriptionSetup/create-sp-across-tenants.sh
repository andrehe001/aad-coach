
# TODO: remove secrets and automate the process completly

az login

az account set --subscription a654ae28-457a-4d2b-9605-1b1c959eea3e 

az ad app create --display-name aad-cost-sp --native-app --available-to-other-tenants true
  appId: a521ec4a-f8fe-4cb0-b54c-93f74a47d88f

az ad sp create --id a521ec4a-f8fe-4cb0-b54c-93f74a47d88f

az ad app credential reset --id a521ec4a-f8fe-4cb0-b54c-93f74a47d88f
  {
    "appId": "a521ec4a-f8fe-4cb0-b54c-93f74a47d88f",
    "name": "a521ec4a-f8fe-4cb0-b54c-93f74a47d88f",
    "password": "LhNqATyW_4KIy-AhDr~ic.NR.KDD5yOa9p",
    "tenant": "c2630420-378e-4563-95a4-519774788b5b"
  }

az role assignment create --assignee a521ec4a-f8fe-4cb0-b54c-93f74a47d88f --role Contributor --scope "subscriptions/a654ae28-457a-4d2b-9605-1b1c959eea3e"

# in each subscription
#########################
az account set --subscription 6bd73137-0103-4141-bf0b-0734f8a5be22
az ad sp create --id a521ec4a-f8fe-4cb0-b54c-93f74a47d88f
az role assignment create --assignee a521ec4a-f8fe-4cb0-b54c-93f74a47d88f --role Contributor --scope "subscriptions/6bd73137-0103-4141-bf0b-0734f8a5be22"

# 6 ging nicht

# Test
#########################
az login --service-principal -u a521ec4a-f8fe-4cb0-b54c-93f74a47d88f -p AZarm_rurggHbsOV_G1w3fUGcYoVUMPQKo --tenant fedbd0c5-d035-4e5e-9844-aedf0f2dd563





