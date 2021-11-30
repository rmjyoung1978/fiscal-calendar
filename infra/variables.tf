variable "region" {
  default     = "eu-west-2"
  description = "aws region"
}

# hosted zone
variable "hosted_zone" {
  type        = string
  description = "name of the hosted zone"
}

variable "base_hosted_zone" {
  type        = string
  description = "name of the base hosted zone"
}

variable "enable_subdomain_delegation" {
  description = "enable sub domain delegation from the root hosted zone for the account"
  default     = true
}

variable "create_hosted_zone" {
  description = "create the hosted zone - this will already exist for prod accounts"
  default     = true
}

# cert
variable "domain_name" {
  type        = string
  description = "the domain name"
}

variable "validation_record_ttl" {
  type        = string
  default     = "300"
  description = "time to live in seconds"
}

# lambda
variable "lambda_bucket_name" {
  type        = string
  description = "bucket that the lambda package is to be uploaded to"
}

variable "api_project_namespace" {
  type        = string
  default     = "FiscalCalendar.Api"
  description = "name of api project - used as a prefix for api project function entrypoint in your code"
}

variable "application" {
  type        = string
  default     = "fiscal-calendar"
  description = "name of the application"
}

variable "environment" {
  type        = string
  description = "the environment the resource is part of e.g. staging, uat etc."
}

variable "owner" {
  type        = string
  default     = "km"
  description = "name of the team responsible for this resource."
}
