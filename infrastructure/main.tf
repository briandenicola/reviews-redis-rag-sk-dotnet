

locals {
  location            = var.region
  resource_name       = "${random_pet.this.id}-${random_id.this.dec}"
  openai_name         = "${local.resource_name}-openai"
  cache_name          = "${local.resource_name}-cache"
  appinsights_name    = "${local.resource_name}-appinsights"
  loganalytics_name   = "${local.resource_name}-logs"
  redis_database_name = "default"
  cache_sku           = "Balanced_B250"
}
