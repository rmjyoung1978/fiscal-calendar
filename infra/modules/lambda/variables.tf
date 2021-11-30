variable "application" {
  type        = string
  description = "the application name"
}

variable "deploy_package_name" {
  type        = string
  description = "name of deployment zip file"
}

variable "lambda_bucket_name" {
  type        = string
  description = "the name of s3 bucket that will be used store the lambda function package"
}

variable "lambda_name" {
  type        = string
  description = "a unique name for your lambda function"
}

variable "lambda_entrypoint" {
  type        = string
  description = "the function entrypoint in your code"
}

variable "lambda_role_arn" {
  type        = string
  description = "arn of the lambda role"
}

variable "runtime" {
  type        = string
  description = "the runtime to be used by the lambda"
  default     = "dotnetcore3.1"
}

variable "memory_size" {
  type        = string
  description = "the amount of memory that can be consumed by the lambda function"
  default     = "1024"
}

variable "timeout" {
  type        = string
  description = "the amount of time your lambda function has to run in seconds"
  default     = "30"
}

variable "retention_in_days" {
  type        = string
  description = "specifies the number of days you want to retain log events in the specified log group."
  default     = "1"
}

variable "environment_variables" {
  description = "environment variables to add to resource"
  type        = map(any)
  default     = {}
}
