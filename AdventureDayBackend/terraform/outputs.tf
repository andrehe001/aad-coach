output "sql_server_connection_string" {
  value     = azurerm_key_vault_secret.kv_secret_sql_administrator_connection_string.value
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

output "admin_server_ip" {
  value = azurerm_public_ip.nginx_ingress_pip.ip_address
}

output "admin_server_url" {
  value = "${azurerm_public_ip.nginx_ingress_pip.domain_name_label}.${azurerm_kubernetes_cluster.aks.location}.cloudapp.azure.com"
}
