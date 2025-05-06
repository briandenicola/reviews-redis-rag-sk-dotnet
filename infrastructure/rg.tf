resource "azurerm_resource_group" "this" {
  name     = "${local.resource_name}_rg"
  location = local.location
  tags = {
    Application = var.tags
    DeployedOn  = timestamp()
    AppName     = local.resource_name
    Tier        = "Azure OpenAI; Azure AI Foundry; Azure Monitor; App Insights; Azure Redis Cache"
  }
}