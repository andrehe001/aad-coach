resource "azurerm_log_analytics_workspace" "la" {
  name                = "${local.prefix_kebab}-la-${local.hash_suffix}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
  retention_in_days   = 30
}


resource "azurerm_log_analytics_solution" "la_monitor_containers_sln" {
  solution_name         = "ContainerInsights"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  workspace_resource_id = azurerm_log_analytics_workspace.la.id
  workspace_name        = azurerm_log_analytics_workspace.la.name

  plan {
    publisher = "Microsoft"
    product   = "OMSGallery/ContainerInsights"
  }
}

resource "azurerm_monitor_diagnostic_setting" "aks_diagnostics" {
  count                      = var.aks_enable_diagnostics ? 1 : 0
  name                       = "${local.prefix_kebab}-aksdiag"
  target_resource_id         = azurerm_kubernetes_cluster.aks.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.la.id

  log {
    category = "kube-apiserver"
    enabled  = true
    retention_policy {
      enabled = false
    }
  }
  log {
    category = "kube-audit"
    enabled  = true
    retention_policy {
      enabled = false
    }
  }
  log {
    category = "cluster-autoscaler"
    enabled  = false
    retention_policy {
      enabled = false
    }
  }
  log {
    category = "kube-scheduler"
    enabled  = false
    retention_policy {
      enabled = false
    }
  }

  log {
    category = "kube-controller-manager"
    enabled  = false
    retention_policy {
      enabled = false
    }
  }

  log {
    category = "kube-apiserver"
    enabled  = false
    retention_policy {
      enabled = false
    }
  }

  metric {
    category = "AllMetrics"
    enabled  = false

    retention_policy {
      days    = 0
      enabled = false
    }
  }
}
