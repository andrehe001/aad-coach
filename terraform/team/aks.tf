# https://www.terraform.io/docs/providers/azurerm/r/kubernetes_cluster.html
resource "azurerm_kubernetes_cluster" "akstf" {
  name                = var.deployment_name
  location            = azurerm_resource_group.aksrg.location
  resource_group_name = azurerm_resource_group.aksrg.name
  dns_prefix          = var.deployment_name
  kubernetes_version  = var.kubernetes_version
  node_resource_group = "${azurerm_resource_group.aksrg.name}_nodes_${azurerm_resource_group.aksrg.location}"

  default_node_pool {
    name               = "default"
    node_count         = 1
    vm_size            = var.vm_size
    os_disk_size_gb    = 120
    max_pods           = 30
    vnet_subnet_id     = azurerm_subnet.aksnet.id
    type               = "VirtualMachineScaleSets"
    node_labels = {
      pool = "default"
      environment = var.deployment_name
    }
    tags = {
      pool = "default"
      environment = var.deployment_name
    }
  }

  role_based_access_control {
    enabled        = true
  }

  network_profile {
      network_plugin = "azure"
      service_cidr   = "10.2.0.0/24"
      dns_service_ip = "10.2.0.10"
      docker_bridge_cidr = "172.17.0.1/16"
      network_policy = "calico"
      load_balancer_sku = "standard"
  }

  identity {
    type = "SystemAssigned"
  }

  # service_principal {
  #   client_id     = azuread_application.aks_app.application_id
  #   client_secret = random_string.aks_sp_password.result
  #   # client_id     = var.aks_client_id
  #   # client_secret = var.aks_client_secret
  # }

  addon_profile {
    oms_agent {
      enabled                    = true
      log_analytics_workspace_id = azurerm_log_analytics_workspace.akslogs.id
    }

    kube_dashboard {
      enabled = false
    }
  }

  tags = {
    environment = var.deployment_name
    network = "azurecni"
    rbac = "true"
    policy = "calico"
  }

  #depends_on = [azurerm_subnet.aksnet, azuread_service_principal.aks_sp, azuread_service_principal_password.aks_sp_set_pw, null_resource.after]
  depends_on = [azurerm_subnet.aksnet]
}

# merge kubeconfig from the cluster
resource "null_resource" "get-credentials" {
  provisioner "local-exec" {
    command = "az aks get-credentials --resource-group ${azurerm_resource_group.aksrg.name} --name ${azurerm_kubernetes_cluster.akstf.name}"
  }
  depends_on = [azurerm_kubernetes_cluster.akstf]
}

output "KUBE_NAME" {
    value = var.deployment_name
}

output "KUBE_GROUP" {
    value = azurerm_resource_group.aksrg.name
}

output "NODE_GROUP" {
  value = azurerm_kubernetes_cluster.akstf.node_resource_group
}

output "ID" {
    value = azurerm_kubernetes_cluster.akstf.id
}

output "HOST" {
  value = azurerm_kubernetes_cluster.akstf.kube_config.0.host
}

output "SERVICE_PRINCIPAL_ID" {
  value = azurerm_kubernetes_cluster.akstf.identity.0.principal_id
}