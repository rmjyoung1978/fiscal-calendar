variable "domain_name" {
  type        = string
  description = "the domain name"
}

variable "subject_alternative_names" {
  type        = list(string)
  default     = []
  description = "dns subject alternative names"
}

variable "validation_record_ttl" {
  type        = string
  default     = "300"
  description = "time to live in seconds"
}

variable "hosted_zone_id" {
  type        = string
  description = "unique id for the hosted zone the certificate will be configured for"
}

variable "application" {
  type        = string
  default     = "fiscal-calendar"
  description = "name of the application"
}

variable "default_tags" {
  description = "a map of tags to add to all resources."
  type        = map(string)
  default     = {}
}
