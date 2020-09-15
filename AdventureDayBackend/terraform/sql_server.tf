resource "random_string" "password" {
  length = 16
  special = false
  min_upper = "2"
  min_lower = "2"
  min_numeric = "2"
}

resource "azurerm_sql_server" "sql_server" {
  name                         = "${local.prefix_kebab}-${local.hash_suffix}"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  
  administrator_login          = "azureuser"
  administrator_login_password = random_string.password.result
}

resource "azurerm_mssql_database" "sql_db" {
  name           = "AzureAdventureDay"
  server_id      = azurerm_sql_server.sql_server.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  max_size_gb    = 64
  read_scale     = false
  sku_name       = "GP_S_Gen5_4"
  zone_redundant = false
}

resource "azurerm_sql_virtual_network_rule" "aks_sql_server_vnet_rule" {
  name                = "sql-vnet-rule"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_sql_server.sql_server.name
  subnet_id           = azurerm_subnet.aks_subnet.id
}

resource "azurerm_sql_firewall_rule" "sql_firewall_all_open" {
  count               = var.leave_sql_server_firewall_open ? 1 : 0
  name                = "firewall-rule-all-open"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_sql_server.sql_server.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "255.255.255.255"
}

resource "azurerm_sql_firewall_rule" "sql_firewall_allow_azure_services" {
  name                = "firewall-rule-azure-services"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_sql_server.sql_server.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_key_vault_secret" "kv_secret_sql_administrator_login" {
  name         = "sql-administrator-login"
  value        = azurerm_sql_server.sql_server.administrator_login
  key_vault_id = azurerm_key_vault.kv.id
  depends_on = [azurerm_key_vault_access_policy.policy_service_principal]
}

resource "azurerm_key_vault_secret" "kv_secret_sql_administrator_login_password" {
  name         = "sql-server-admin-password"
  value        = azurerm_sql_server.sql_server.administrator_login_password
  key_vault_id = azurerm_key_vault.kv.id
  depends_on = [azurerm_key_vault_access_policy.policy_service_principal]
}

resource "azurerm_key_vault_secret" "kv_secret_sql_administrator_connection_string" {
  name         = "sql-server-connection-string"
  value        = "Server=tcp:${azurerm_sql_server.sql_server.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.sql_db.name};Persist Security Info=False;User ID=${azurerm_sql_server.sql_server.administrator_login};Password=${azurerm_sql_server.sql_server.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.kv.id
  depends_on = [azurerm_key_vault_access_policy.policy_service_principal]
}

resource "azurerm_key_vault_secret" "kv_secret_sql_server_host_name" {
  name         = "sql-server-host-name"
  value        = azurerm_sql_server.sql_server.fully_qualified_domain_name
  key_vault_id = azurerm_key_vault.kv.id
  depends_on   = [azurerm_key_vault_access_policy.policy_service_principal]
}