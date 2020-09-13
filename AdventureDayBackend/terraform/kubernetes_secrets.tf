
resource "kubernetes_secret" "sql_server_secret" {
  metadata {
    name = "sql-server-secret"
    namespace = kubernetes_namespace.adventure_day_namespace.metadata[0].name
  }

  data = {
    connection_string = azurerm_key_vault_secret.kv_secret_sql_administrator_connection_string.value
  }

  type = "Opaque"
}