resource "aws_s3_bucket_object" "lambda_package" {
  key    = "${var.application}/${var.deploy_package_name}"
  bucket = var.lambda_bucket_name
  source = "package/${var.deploy_package_name}"
  etag   = md5(filebase64("package/${var.deploy_package_name}"))
}

resource "aws_lambda_function" "lambda_function" {
  depends_on       = [aws_s3_bucket_object.lambda_package]
  s3_bucket        = var.lambda_bucket_name
  s3_key           = "${var.application}/${var.deploy_package_name}"
  function_name    = var.lambda_name
  role             = var.lambda_role_arn
  handler          = var.lambda_entrypoint
  runtime          = var.runtime
  memory_size      = var.memory_size
  timeout          = var.timeout
  source_code_hash = filebase64sha256("package/${var.deploy_package_name}")
  environment {
    variables = var.environment_variables
  }
}

resource "aws_cloudwatch_log_group" "lambda_log_group" {
  depends_on        = [aws_lambda_function.lambda_function]
  name              = "/aws/lambda/${var.lambda_name}"
  retention_in_days = var.retention_in_days
}
