output "sql_server_connection_string" {
  value = azurerm_key_vault_secret.kv_secret_sql_administrator_connection_string.value
  sensitive = true
}