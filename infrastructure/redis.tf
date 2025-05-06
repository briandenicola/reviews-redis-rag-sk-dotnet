resource "azapi_resource" "cache" {
  schema_validation_enabled = false
  type                      = "Microsoft.Cache/redisEnterprise@2025-05-01-preview"
  name                      = local.cache_name
  parent_id                 = azurerm_resource_group.this.id
  identity {
    type = "SystemAssigned"
  }
  location = azurerm_resource_group.this.location

  body = {
    sku = {
      name = local.cache_sku
    }
    properties = {
    }
  }
}

resource "azurerm_monitor_diagnostic_setting" "app_insight" {
  name                       = "${local.cache_name}-diag"
  target_resource_id         = azapi_resource.cache.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.this.id

  metric {
    category = "AllMetrics"
  }
}

resource "azapi_resource" "database" {
  schema_validation_enabled = false
  type                      = "Microsoft.Cache/redisEnterprise/databases@2025-05-01-preview"
  name                      = local.redis_database_name
  parent_id                 = azapi_resource.cache.id
  body = {
    properties = {
      accessKeysAuthentication = "Enabled"
      clientProtocol           = "Plaintext"
      clusteringPolicy         = "EnterpriseCluster"
      deferUpgrade             = "NotDeferred"
      evictionPolicy           = "NoEviction"
      modules = [
        {
          name = "RediSearch"
        },
        {
          name = "RedisJSON"
        }        
      ]
    }
  }
}

data "azurerm_redis_enterprise_database" "this" {
  name                = "default"
  cluster_id          = azapi_resource.cache.id
}
