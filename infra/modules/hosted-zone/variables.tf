variable "hosted_zone" {
  type        = string
  description = "the name of the hosted zone"
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

variable "application" {
  type        = string
  description = "the application name"
}

variable "default_tags" {
  description = "a map of tags to add to all resources."
  type        = map(string)
  default     = {}
}
