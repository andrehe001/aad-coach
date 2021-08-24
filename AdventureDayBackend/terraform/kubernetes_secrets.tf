resource "kubernetes_secret" "sql_server_secret" {
  metadata {
    name      = "sql-server-secret"
    namespace = kubernetes_namespace.adventure_day_namespace.metadata[0].name
  }

  data = {
    connection_string = azurerm_key_vault_secret.kv_secret_sql_administrator_connection_string.value
  }

  type = "Opaque"
}

resource "kubernetes_secret" "azure_sp" {
  metadata {
    name      = "azure-sp"
    namespace = kubernetes_namespace.adventure_day_namespace.metadata[0].name
  }

  data = {
    client_id     = azurerm_key_vault_secret.kv_secret_azure_sp_client_id.value
    client_secret = azurerm_key_vault_secret.kv_secret_azure_sp_client_secret.value
  }

  type = "Opaque"
}
