variable "application" {
  type        = string
  description = "The application name"
}

variable "metric_name" {
  type        = string
  description = "name of the waf rule metric name, cannot contain special characters"
}
