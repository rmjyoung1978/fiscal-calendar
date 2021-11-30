variable "application" {
  type        = string
  description = "The application name"
}

variable "lambda_name" {
  type        = string
  description = "A unique name for your Lambda Function"
}

variable "invoke_arn" {
  type        = string
  description = "The lambda invoke arn by the api-gateway"
}

variable "arn" {
  type        = string
  description = "The lambda arn by the api-gateway"
}

variable "waf_web_acl_id" {
  type        = string
  description = "id of the web acl of the waf rules created"
}
