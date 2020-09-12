output "sql_server_connection_string" {
  value = azurerm_key_vault_secret.kv_secret_sql_administrator_connection_string.value
  sensitive = true
}

output "aks_name" {
  value = azurerm_kubernetes_cluster.aks.name
}

output "aks_resource_group_name" {
  value = azurerm_kubernetes_cluster.aks.resource_group_name
}

output "acr_name" {
  value = azurerm_container_registry.acr.name
}