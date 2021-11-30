terraform {
  required_version = "~> 1.0.4"
  backend "s3" {
    encrypt = true
  }
  required_providers {
    aws = {
      version = "~> 3.53.0"
    }
  }
}

provider "aws" {
  region = var.region
  default_tags {
    tags = local.default_tags
  }
}
