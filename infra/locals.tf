locals {
  default_tags = {
    "application" = var.application
    "environment" = var.environment
    "owner"       = var.owner
  }

  deploy_package_name = "deploy-api-package.zip"
}
