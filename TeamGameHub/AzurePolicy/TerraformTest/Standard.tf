resource "azurerm_sql_server" "gamesqlserver3" {
  name                         = "game-sqlserver-standard"
  resource_group_name          = "policytest-rg"
  location                     = "northeurope"
  version                      = "12.0"
  administrator_login          = "gamedbadministrator"
  administrator_login_password = "lsdöjdsfölÖLSDFK!4"


}

resource "azurerm_mssql_database" "gamedb3" {
  name           = "gamedb"
  server_id      = azurerm_sql_server.gamesqlserver3.id
  sku_name       = "S0"
  max_size_gb    = 250
}