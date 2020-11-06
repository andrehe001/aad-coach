# Setting up the Adventure Day PROD Backend

## Prerequisites

* Admin subscription is setup
* Service Principal for admin subscription is setup and credentials are known

## Updating the Service Principal Details

* Navigate to this project's Settings [Secrets config page](https://github.com/azure-adventure-day/aad-coach/settings/secrets/actions).
* Update the Secret with the name `AZURE_PROD` with the following template (replace `<GUID>` accordingly):

```json
 {
    "clientId": "<GUID>",
    "clientSecret": "<GUID>",
    "subscriptionId": "<GUID>",
    "tenantId": "<GUID>"
  }
```

## Executing the GitHub Actions

## 1 - Infrastructure as Code Deployment
1. Navigate to this project's [GitHub Actions page](https://github.com/azure-adventure-day/aad-coach/actions)
2. Navigate to the action [AdventureDay-Backend-IaC](https://github.com/azure-adventure-day/aad-coach/actions?query=workflow%3AAdventureDay-Backend-IaC). 
Changes to the attribute are only required if you need to change the `location`. Otherwise directly execute the `Run workflow` command:
![Image of GH Action Workflow for Backend-IaC](./imgs/gh-action-backend-iac.png)

## 2 - Backend Application Deployment
1. Navigate to this project's [GitHub Actions page](https://github.com/azure-adventure-day/aad-coach/actions)
2. Navigate to the action [AdventureDay-Backend-Build-Deploy](https://github.com/azure-adventure-day/aad-coach/actions?query=workflow%3AAdventureDay-Backend-Build-Deploy). 
Update the `Azure Container Registry Name` (2) with the name of the deployed ACR instance from the *Infrastructure as Code Pipeline*. Finally execute the `Run workflow` (2) command:
![Image of GH Action Workflow for Backend-IaC](./imgs/gh-action-deploy-backend.png)


## Initializing the Databases

**TODO** - describe how to get the DB Connection String from the KV or introduce another pipeline to automate also the DB script deployment.

## Verify Deployment

**TODO** - spit out the URL as the result of the backend deployment run

## Other TODOs

* Avoid having to update the ACR name in the setup. 
* Cost SP needs to be configured automatically.
