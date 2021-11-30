output "base_url" {
  value = aws_api_gateway_stage.api_stage.invoke_url
}

output "stage_name" {
  value = aws_api_gateway_stage.api_stage.stage_name
}

output "api_key_value" {
  value = aws_api_gateway_api_key.api_key.value
}

output "rest_api_id" {
  value = aws_api_gateway_rest_api.rest_api.id
}