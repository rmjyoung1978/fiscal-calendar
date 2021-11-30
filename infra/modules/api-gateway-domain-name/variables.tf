variable "domain_name" {
  type        = string
  description = "the domin name"
}

variable "certificate_arn" {
  type        = string
  description = "certificate arm"
}

variable "api_gateway_id" {
  type        = string
  description = "api gateway id"
}

variable "api_gateway_stage_name" {
  type        = string
  description = "api gateway deployed stage name"
}

variable "hosted_zone_id" {
  type        = string
  description = "the hosted zone id"
}