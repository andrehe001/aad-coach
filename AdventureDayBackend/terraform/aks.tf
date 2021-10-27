data "azurerm_kubernetes_service_versions" "current" {
  location       = var.location
  version_prefix = var.aks_kubernetes_version_prefix
}

locals {
  # vnet           10.0.0.0/16 -> IP Range: 10.0.0.1 - 10.0.255.254
  # aks            10.0.0.0/20 -> IP Range: 10.0.0.1 - 10.0.15.254
  # aks services   10.1.0.0/20 -> IP Range: 10.1.0.1 - 10.0.15.254
  # docker bridge  172.17.0.1/16
  # firewall       10.0.240.0/24 -> IP Range: 10.0.240.1 - 10.0.240.254
  vnet_cidr            = "10.0.0.0/16"
  aks_subnet_cidr      = "10.0.0.0/20"
  aks_service_cidr     = "10.1.0.0/20"
  aks_dns_service_ip   = "10.1.0.10"
  docker_bridge_cidr   = "172.17.0.1/16"
  firewall_subnet_cidr = "10.0.240.0/26"  // Placeholder Subnet for egress lockdown via AzFW
  app_gw_subnet_cidr   = "10.0.240.64/26" // Placeholder Subnet for AppGW Ingress
}

resource "azurerm_virtual_network" "vnet" {
  name                = "${local.prefix_kebab}-vnet"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  address_space       = ["${local.vnet_cidr}"]

}

resource "azurerm_subnet" "aks_subnet" {
  name                 = "aks_subnet"
  resource_group_name  = azurerm_resource_group.rg.name
  address_prefixes     = [local.aks_subnet_cidr]
  virtual_network_name = azurerm_virtual_network.vnet.name
  service_endpoints    = var.aks_subnet_service_endpoints
}

resource "azurerm_kubernetes_cluster" "aks" {
  
  lifecycle {
    ignore_changes = [
      default_node_pool[0].node_count, tags
    ]
  }

  name                = local.prefix_kebab
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  default_node_pool {
    name                = var.aks_default_node_pool.name
    node_count          = var.aks_default_node_pool.node_count
    vm_size             = var.aks_default_node_pool.vm_size
    availability_zones  = var.aks_default_node_pool.availability_zones
    node_labels         = var.aks_default_node_pool.node_labels
    node_taints         = var.aks_default_node_pool.node_taints
    enable_auto_scaling = var.aks_default_node_pool.cluster_auto_scaling
    max_count           = var.aks_default_node_pool.cluster_auto_scaling_max_count
    min_count           = var.aks_default_node_pool.cluster_auto_scaling_min_count

    enable_node_public_ip = false

    os_disk_size_gb = 128
    max_pods        = 250

    vnet_subnet_id = azurerm_subnet.aks_subnet.id
  }

  dns_prefix = "${local.prefix_kebab}-aks-${local.hash_suffix}"

  addon_profile {
    kube_dashboard {
      enabled = false
    }

    oms_agent {
      enabled                    = true
      log_analytics_workspace_id = azurerm_log_analytics_workspace.la.id
    }

    azure_policy {
      enabled = var.aks_enable_azure_policy_support
    }
  }

  api_server_authorized_ip_ranges = []

  # Use a Managed Identity
  identity {
    type = "SystemAssigned"
  }

  kubernetes_version = data.azurerm_kubernetes_service_versions.current.latest_version

  network_profile {
    network_plugin     = "azure"
    service_cidr       = local.aks_service_cidr
    docker_bridge_cidr = local.docker_bridge_cidr
    dns_service_ip     = local.aks_dns_service_ip
    load_balancer_sku  = "standard"
    load_balancer_profile {
      managed_outbound_ip_count = 1
      outbound_ports_allocated  = 5000
    }
  }

  node_resource_group = "${local.prefix_snake}_aks_nodes_rg"

  role_based_access_control {
    azure_active_directory {
      managed                = true
      admin_group_object_ids = var.aks_admin_group_object_ids
    }

    enabled = true
  }

  sku_tier = var.aks_sku_tier
}

resource "azurerm_kubernetes_cluster_node_pool" "aks_node_pool" {
  lifecycle {
    ignore_changes = [
      node_count
    ]
  }

  for_each = var.aks_additional_node_pools

  kubernetes_cluster_id = azurerm_kubernetes_cluster.aks.id
  name                  = substr(each.key, 0, 12)
  node_count            = each.value.node_count
  vm_size               = each.value.vm_size
  availability_zones    = each.value.availability_zones
  max_pods              = 250
  os_disk_size_gb       = 128
  os_type               = "Linux"
  vnet_subnet_id        = azurerm_subnet.aks_subnet.id
  node_taints           = each.value.node_taints
  node_labels           = each.value.node_labels
  enable_auto_scaling   = each.value.cluster_auto_scaling
  min_count             = each.value.cluster_auto_scaling_min_count
  max_count             = each.value.cluster_auto_scaling_max_count
}
