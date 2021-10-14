# Setting up the Adventure Day PROD Backend

## Prerequisites

* Admin subscription is setup
* Service Principal for admin subscription with Owner rights is created and credentials are known
  * Can be created with `az ad sp create-for-rbac --sdk-auth --role Owner --scopes /subscriptions/<GUID>`

## Executing the Deployment for production

1. Navigate to this project's [GitHub Actions page](https://github.com/azure-adventure-day/aad-coach/actions)
2. Navigate to the action [Backend-IaC-Build-Deploy](https://github.com/azure-adventure-day/aad-coach/actions/workflows/adventure-day-backend-iac-build-deploy.yml).
3. Enter the Azure Service Principal Client ID, Client Secret, Azure Subscription ID and Tenant ID.
4. Changes to the other attributes are only required if you need to change the `location` or for example have multiple backends in one Azure Subscription.
5. Execute the `Run workflow` command.

## Executing the Deployment for development/testing

> **Only Core Team, as you need to have a specified GitHub secret configured**

To speed up testing during development, it is possible to use a GitHub secret for deployment:

1. Create SP with `az ad sp create-for-rbac --sdk-auth --role Owner --scopes /subscriptions/<GUID>`
2. When complete, the command displays JSON output in the following form:

  ```json
  {
    "clientId": "<GUID>",
    "clientSecret": "<CLIENT_SECRET_VALUE>",
    "subscriptionId": "<GUID>",
    "tenantId": "<GUID>",
    (...)
  }
  ```

3. At [Secrets](https://github.com/azure-adventure-day/aad-coach/settings/secrets/actions) create a new secret named `AZURE_YOUR_ALIAS` with this output.

> **NOTE: While adding secret `AZURE_YOUR_ALIAS` make sure to add like this**

```json
{"clientId": "<GUID>",
  "clientSecret": "<CLIENT_SECRET_VALUE>",
  "subscriptionId": "<GUID>",
  "tenantId": "<GUID>",
  (...)}
```

   instead of  

```json
{
  "clientId": "<GUID>",
  "clientSecret": "<CLIENT_SECRET_VALUE>",
  "subscriptionId": "<GUID>",
  "tenantId": "<GUID>",
  (...)
}
```

> **to prevent unnecessary masking of `{ }` in our logs.**
     
4. Navigate to the action [Backend-IaC-Build-Deploy](https://github.com/azure-adventure-day/aad-coach/actions/workflows/adventure-day-backend-iac-build-deploy.yml).
5. Enter the GitHub Secret name you have chosen
6. Changes to the other attributes are only required if you need to change the `location` or for example have multiple backends in one Azure Subscription.
7. Execute the `Run workflow` command.
