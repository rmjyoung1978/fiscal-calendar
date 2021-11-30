resource "aws_api_gateway_rest_api" "rest_api" {
  name        = "${var.lambda_name}-gateway"
  description = "api gateway for ${var.application} environment"
  endpoint_configuration {
    types = ["REGIONAL"]
  }
}

resource "aws_api_gateway_resource" "proxy" {
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  parent_id   = aws_api_gateway_rest_api.rest_api.root_resource_id
  path_part   = "{proxy+}"
}

resource "aws_api_gateway_method" "proxy" {
  rest_api_id      = aws_api_gateway_rest_api.rest_api.id
  resource_id      = aws_api_gateway_resource.proxy.id
  http_method      = "ANY"
  authorization    = "NONE"
  api_key_required = "true"
}

resource "aws_api_gateway_integration" "lambda" {
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  resource_id = aws_api_gateway_method.proxy.resource_id
  http_method = aws_api_gateway_method.proxy.http_method

  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = var.invoke_arn
}

resource "aws_api_gateway_method_response" "response_200" {
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  resource_id = aws_api_gateway_resource.proxy.id
  http_method = aws_api_gateway_method.proxy.http_method
  status_code = "200"
}

resource "aws_api_gateway_deployment" "calendar_api" {
  depends_on        = [aws_api_gateway_integration.lambda]
  rest_api_id       = aws_api_gateway_rest_api.rest_api.id
  stage_description = "Deployed at ${timestamp()}"
  triggers = {
    redeployment = sha1(jsonencode([
      aws_api_gateway_resource.proxy.id,
      aws_api_gateway_method.proxy.id,
    aws_api_gateway_integration.lambda.id]))
  }
  lifecycle {
    create_before_destroy = true
  }
}

resource "aws_api_gateway_stage" "api_stage" {
  depends_on    = [aws_api_gateway_deployment.calendar_api]
  deployment_id = aws_api_gateway_deployment.calendar_api.id
  rest_api_id   = aws_api_gateway_rest_api.rest_api.id
  stage_name    = "Main"
}

resource "aws_api_gateway_method_settings" "api_logging" {
  depends_on  = [aws_api_gateway_method.proxy]
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  stage_name  = aws_api_gateway_stage.api_stage.stage_name
  method_path = "*/*"

  settings {
    logging_level      = "INFO"
    data_trace_enabled = true
    metrics_enabled    = true
  }
}

resource "aws_lambda_permission" "apigw" {
  statement_id  = "allow-api-gateway-${var.lambda_name}-to-invoke-lambda"
  action        = "lambda:InvokeFunction"
  function_name = var.arn
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_api_gateway_rest_api.rest_api.execution_arn}/*/*/*"
}

resource "aws_api_gateway_usage_plan" "api_usage_plan" {
  name = "${var.lambda_name}_api_usage_plan"
  api_stages {
    api_id = aws_api_gateway_rest_api.rest_api.id
    stage  = aws_api_gateway_stage.api_stage.stage_name
  }
}

resource "aws_api_gateway_api_key" "api_key" {
  name = "${var.lambda_name}-api-key"
}

resource "aws_api_gateway_usage_plan_key" "main" {
  key_id        = aws_api_gateway_api_key.api_key.id
  key_type      = "API_KEY"
  usage_plan_id = aws_api_gateway_usage_plan.api_usage_plan.id
}

resource "aws_wafregional_web_acl_association" "waf_api_gateway_association" {
  depends_on   = [aws_api_gateway_stage.api_stage]
  resource_arn = aws_api_gateway_stage.api_stage.arn
  web_acl_id   = var.waf_web_acl_id
}
