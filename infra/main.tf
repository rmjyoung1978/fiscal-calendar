# hosted zone
module "main_hosted_zone" {
  source      = "./modules/hosted-zone"
  application = var.application

  hosted_zone                 = var.hosted_zone
  base_hosted_zone            = var.base_hosted_zone
  enable_subdomain_delegation = var.enable_subdomain_delegation
  create_hosted_zone          = var.create_hosted_zone
  default_tags                = local.default_tags
}

# waf rules
module "waf" {
  source      = "./modules/waf"
  application = var.application

  metric_name = replace(var.application, "-", "")
}

# certificate
module "certificate_eu_west_2" {
  source      = "./modules/cert-creation-validation"
  application = var.application

  domain_name           = var.domain_name
  validation_record_ttl = var.validation_record_ttl
  hosted_zone_id        = module.main_hosted_zone.hosted_zone_id
  default_tags          = local.default_tags
}

# iam role
module "lambda_iam_role" {
  source      = "./modules/iam-role"
  application = var.application
}

# lambda
module "lambda_api" {
  source      = "./modules/lambda"
  application = var.application

  lambda_name         = "${var.application}-api"
  lambda_entrypoint   = "${var.api_project_namespace}::${var.api_project_namespace}.LambdaEntryPoint::FunctionHandlerAsync"
  lambda_role_arn     = module.lambda_iam_role.arn
  deploy_package_name = local.deploy_package_name
  lambda_bucket_name  = var.lambda_bucket_name
  environment_variables = {
    ASPNETCORE_ENVIRONMENT = var.environment
  }
}

# api gateway
module "api_gateway_api" {
  source      = "./modules/api-gateway"
  depends_on  = [module.lambda_api, module.waf]
  application = var.application

  lambda_name    = module.lambda_api.name
  arn            = module.lambda_api.arn
  invoke_arn     = module.lambda_api.invoke_arn
  waf_web_acl_id = module.waf.waf_web_acl_id
}

module "api_gateway_api_domain_name" {
  source     = "./modules/api-gateway-domain-name"
  depends_on = [module.api_gateway_api]

  domain_name            = var.domain_name
  certificate_arn        = module.certificate_eu_west_2.arn
  api_gateway_id         = module.api_gateway_api.rest_api_id
  api_gateway_stage_name = module.api_gateway_api.stage_name
  hosted_zone_id         = module.main_hosted_zone.hosted_zone_id
}
