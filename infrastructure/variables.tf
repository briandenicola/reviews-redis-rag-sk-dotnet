variable "region" {
  description = "Region to deploy resources to"
  default     =  "eastus2"
}

variable "tags" {
  description = "Tags to apply to Resource Group"
}

variable "completions_model" {
  description = "The Completions LLM model to use"
  type = object({
    name            = string
    deployment_name = string
    version         = string
    sku_type        = string
  })
  default = {
    name            = "gpt-4o"
    deployment_name = "gpt-4o"
    version         = "2024-08-06"
    sku_type        = "GlobalStandard"
  }
}

variable "embedding_model" {
  description = "The Embedding LLM model to use"
  type = object({
    name            = string
    deployment_name = string
    version         = string
    sku_type        = string
  })
  default = {
    name            = "gpt-4"
    deployment_name = "gpt-4"
    version         = "0613"
    sku_type        = "GlobalStandard"
  }
}