resource "azurerm_key_vault_secret" "kv_secret_azure_sp_client_id" {
  name         = "azure-sp-client-id"
  value        = var.azure_sp_client_id
  key_vault_id = azurerm_key_vault.kv.id
  depends_on   = [azurerm_key_vault_access_policy.policy_service_principal]
}

resource "azurerm_key_vault_secret" "kv_secret_azure_sp_client_secret" {
  name         = "azure-sp-client-secret"
  value        = var.azure_sp_client_secret
  key_vault_id = azurerm_key_vault.kv.id
  depends_on   = [azurerm_key_vault_access_policy.policy_service_principal]
}

