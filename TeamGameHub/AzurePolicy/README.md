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
az provider register --namespace 'Microsoft.PolicyInsights'

az policy definition create --name 'sql-db-skus' --display-name 'Allowed SQL DB SKUs' --description 'This policy enables you to specify a set of SQL DB SKUs' --rules 'azurepolicy.rules.json' --params 'azurepolicy.parameters.json' --mode All

az policy assignment create --name 'deny-sql-db-skus' --scope '/subscriptions/{subId}/' --policy "sql-db-skus" --params "{'listofSKUName':{'value':['GeneralPurpose']}}"
````