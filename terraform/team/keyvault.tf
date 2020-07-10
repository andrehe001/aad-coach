resource "azurerm_key_vault" "aksvauls" {
  name                        = "${var.deployment_name}-vault"
  location                    = azurerm_resource_group.aksrg.location
  resource_group_name         = azurerm_resource_group.aksrg.name
  enabled_for_disk_encryption = false
  tenant_id                   = var.tenant_id

  sku_name = "standard"

  network_acls {
    default_action = "Deny"
    bypass         = "AzureServices"
  }

  tags = {
    environment = var.deployment_name
  }
}