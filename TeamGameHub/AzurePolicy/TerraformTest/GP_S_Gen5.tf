resource "azurerm_sql_server" "gamesqlserver1" {
  name                         = "game-sqlserver-gen5-s"
  resource_group_name          = "policytest-rg"
  location                     = "northeurope"
  version                      = "12.0"
  administrator_login          = "gamedbadministrator"
  administrator_login_password = "lsdöjdsfölÖLSDFK!4"


}

resource "azurerm_mssql_database" "gamedb1" {
  name           = "gamedb"
  server_id      = azurerm_sql_server.gamesqlserver1.id
  max_size_gb    = 32
  sku_name       = "GP_S_Gen5_1"
  min_capacity   = 1
  auto_pause_delay_in_minutes = 60
}