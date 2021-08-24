resource "azurerm_key_vault" "kv" {
  // ${substr(local.prefix_flat, 0, min(18, length(local.prefix_flat)))}${local.hash_suffix}
  name                        = "${substr("${local.prefix_kebab}", 0, min(17, length("${local.prefix_kebab}-${local.hash_suffix}")))}-${local.hash_suffix}"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  purge_protection_enabled    = false
  sku_name                    = "standard"
}

resource "azurerm_key_vault_access_policy" "policy_service_principal" {
  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id
  key_permissions = [
    # Possible values: backup, create, decrypt, delete, encrypt, get, import, list, purge, recover, restore, sign, unwrapKey, update, verify and wrapKey.
    "get", "list", "update", "delete",
  ]

  secret_permissions = [
    # Possible values: backup, delete, get, list, purge, recover, restore and set.
    "get", "list", "set", "delete",
  ]

  certificate_permissions = [
    # Possible value: backup, create, delete, deleteissuers, get, getissuers, import, list, listissuers, managecontacts, manageissuers, purge, recover, restore, setissuers and update.
    "create", "get", "list", "update", "delete",
  ]
}

resource "azurerm_key_vault_access_policy" "policy_users" {
  count = length(var.key_vault_users_object_ids)

  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id

  object_id = var.key_vault_users_object_ids[count.index]

  key_permissions = [
    # Possible values: backup, create, decrypt, delete, encrypt, get, import, list, purge, recover, restore, sign, unwrapKey, update, verify and wrapKey.
    "get", "list", "update", "delete",
  ]

  secret_permissions = [
    # Possible values: backup, delete, get, list, purge, recover, restore and set.
    "get", "list", "set", "delete",
  ]

  certificate_permissions = [
    # Possible value: backup, create, delete, deleteissuers, get, getissuers, import, list, listissuers, managecontacts, manageissuers, purge, recover, restore, setissuers and update.
    "create", "get", "list", "update", "delete",
  ]
}
