resource "aws_api_gateway_domain_name" "domain_name" {
  domain_name              = var.domain_name
  regional_certificate_arn = var.certificate_arn

  endpoint_configuration {
    types = ["REGIONAL"]
  }
}

resource "aws_api_gateway_base_path_mapping" "base_mapping" {
  api_id      = var.api_gateway_id
  domain_name = aws_api_gateway_domain_name.domain_name.domain_name
  stage_name  = var.api_gateway_stage_name
}

resource "aws_route53_record" "routeAlias" {
  name    = aws_api_gateway_domain_name.domain_name.domain_name
  type    = "A"
  zone_id = var.hosted_zone_id

  alias {
    evaluate_target_health = false
    name                   = aws_api_gateway_domain_name.domain_name.regional_domain_name
    zone_id                = aws_api_gateway_domain_name.domain_name.regional_zone_id
  }
}
