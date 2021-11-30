output "hosted_zone_id" {
  value = var.create_hosted_zone ? aws_route53_zone.main_zone[0].zone_id : data.aws_route53_zone.app_zone[0].zone_id
}

