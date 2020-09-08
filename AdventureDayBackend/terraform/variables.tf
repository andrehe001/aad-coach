variable "prefix" {
  type = string
  description = "Injected via tf.ps1. Resource prefix."
}

variable "location" {
  type = string
  description = "Injected via tf.ps1. Resource location."
}

variable "aks_kubernetes_version_prefix" {
  type        = string
  default     = "1.17"
  description = "The Kubernetes Version prefix (MAJOR.MINOR) to be used by the AKS cluster. The BUGFIX version is determined automatically (latest)."
}

variable "aks_default_node_pool" {
  description = "Configuration for the default node pool."
  type = object({
    name                           = string
    node_count                     = number
    vm_size                        = string
    availability_zones             = list(string)
    node_labels                    = map(string)
    node_taints                    = list(string)
    cluster_auto_scaling           = bool
    cluster_auto_scaling_min_count = number
    cluster_auto_scaling_max_count = number
  })

  default = {
    name                           = "default",
    node_count                     = 1,
    vm_size                        = "Standard_DS2_v2"
    availability_zones             = [],
    node_labels                    = {},
    node_taints                    = [],
    cluster_auto_scaling           = false,
    cluster_auto_scaling_min_count = null,
    cluster_auto_scaling_max_count = null
  }
}

variable "aks_additional_node_pools" {
  description = "The map object to configure one or several additional node pools."
  type = map(object({
    node_count                     = number
    vm_size                        = string
    availability_zones             = list(string)
    node_labels                    = map(string)
    node_taints                    = list(string)
    cluster_auto_scaling           = bool
    cluster_auto_scaling_min_count = number
    cluster_auto_scaling_max_count = number
  }))

  default = {}
}

variable "aks_sku_tier" {
  type        = string
  default     = "Free"
  description = " The SKU Tier that should be used for this Kubernetes Cluster. Changing this forces a new resource to be created. Possible values are Free and Paid."
}

variable "aks_enable_azure_policy_support" {
  type        = bool
  default     = false
  description = "Enable AKS Policy Support. Currently in Preview and not supported for production workloads. Requires a whitelisted subscription."
}

variable "aks_subnet_service_endpoints" {
  type    = list(string)
  default = ["Microsoft.Storage", "Microsoft.KeyVault", "Microsoft.Sql", "Microsoft.AzureCosmosDB"]
}

variable "aks_api_server_authorized_ip_ranges" {
  type        = list(string)
  default     = ["0.0.0.0/0"]
  description = "The IP ranges to whitelist for incoming traffic to the masters."
}

variable "aks_admin_group_object_ids" {
  type        = list(string)
  default     = []
  description = "The cluster-admins for the Kubernetes cluster."
}

variable "aks_enable_diagnostics" {
  type        = bool
  default     = false
  description = "Activate AKS Diagnostics Settings in Log Analytics"
}
