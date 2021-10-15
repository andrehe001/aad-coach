# Import Teams, Azure Subscriptions and Accounts

## Prerequisites

* Subscriptions and Azure Accounts need to be in place and their info needs to be available in an Excel file conforming to the [AzureAdventureDay_AzurePreparation_Template.xlsx](./AzureAdventureDay_AzurePreparation_Template.xlsx).
* The backend was already deployed and the portal can be logged in to.

## Team import

> **Warning**: Team imports via Excel can be performed multiple times, but **all existing teams, including scores and team members** are deleted during the process. Thus, the import should ***NEVER*** be performed once an event has started!

0. Log in as admin to the portal and navigate to the *AdministrationTeamsImport* page.
1. Click **Choose file** and select your *AzureAdventureDay_AzurePreparation_XYZ.xlsx* file.
2. Click **Start file upload...** and wait for the result. In case of errors that will prevent the upload as a whole, these will be displayed immediately.

   Otherwise, the result should look similar to this:

   ![AdministrationTeamsImport page](./media/team-import-page.png)
