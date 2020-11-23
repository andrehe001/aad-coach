# Allowed SQL DB SKUs

This policy enables you to specify a set of SQL DB SKUs

## with PowerShell

````powershell
Register-AzResourceProvider -ProviderNamespace 'Microsoft.PolicyInsights'

$definition = New-AzPolicyDefinition -Name "sql-db-skus" -DisplayName "Allowed SQL DB SKUs" -description "This policy enables you to specify a set of SQL DB SKUs" -Policy 'azurepolicy.rules.json' -Parameter 'azurepolicy.parameters.json' -Mode All
$definition

$policyparam = '{ "listOfSKUName": { "value": [ "GeneralPurpose" ] } }'

$assignment = New-AzPolicyAssignment -Name "deny-sql-db-skus" -Scope "/subscriptions/{subId}/"   -PolicyParameter $policyparam -PolicyDefinition $definition
$assignment 
````

## with CLI

````cli
az login

az account set --subscription a4baed79-40f8-4697-a6b2-7f16d29feb8b

az provider register --namespace 'Microsoft.PolicyInsights'

az policy definition create --name 'sql-db-skus-new' --display-name 'Allowed only SQL DB GP' --description 'This policy only allows GeneralPurpose SQL DB SKU' --rules 'azurepolicy.rules3.json' --mode All

az policy assignment create --name 'deny-sql-db-skus' --scope '/subscriptions/a4baed79-40f8-4697-a6b2-7f16d29feb8b/' --policy sql-db-skus-new

az policy state trigger-scan --resource-group policytest-rg



````


## Test with Terraform

```
az login


az account set --subscription a4baed79-40f8-4697-a6b2-7f16d29feb8b

az group create -n policytest-rg -l northeurope

terraform init

terraform apply .


az sql db create -g policytest-rg -s game-sqlserver-gen5 -n mydb -e GeneralPurpose -f Gen5 -c 2
```