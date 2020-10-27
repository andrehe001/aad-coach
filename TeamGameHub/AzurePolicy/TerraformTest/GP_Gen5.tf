resource "azurerm_sql_server" "gamesqlserver2" {
  name                         = "game-sqlserver-gen5"
  resource_group_name          = "policytest-rg"
  location                     = "northeurope"
  version                      = "12.0"
  administrator_login          = "gamedbadministrator"
  administrator_login_password = "lsdöjdsfölÖLSDFK!4"


}

resource "azurerm_mssql_database" "gamedb2" {
  name           = "gamedb"
  server_id      = azurerm_sql_server.gamesqlserver2.id
  max_size_gb    = 32
  sku_name       = "GP_Gen5_2"
}