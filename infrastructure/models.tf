resource "azurerm_cognitive_deployment" "embeddings" {
  name                 = var.embedding_model.deployment_name
  cognitive_account_id = azurerm_cognitive_account.this.id
  model {
    format  = "OpenAI"
    name    = var.embedding_model.name
    version = var.embedding_model.version
  }

  sku {
    name     = var.embedding_model.sku_type
    capacity = 15
  }

}

resource "azurerm_cognitive_deployment" "completions" {
  name                 = var.completions_model.deployment_name
  cognitive_account_id = azurerm_cognitive_account.this.id
  model {
    format  = "OpenAI"
    name    = var.completions_model.name
    version = var.completions_model.version
  }

  sku {
    name     = var.completions_model.sku_type
    capacity = 15
  }
}