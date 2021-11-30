resource "aws_route53_zone" "main_zone" {
  count   = var.create_hosted_zone ? 1 : 0
  name    = var.hosted_zone
  comment = "hosted zone for ${var.application} environment"
  tags = merge(
    var.default_tags,
    {
      "Name" = "${var.application}-hosted-zone"
    }
  )

  lifecycle {
    ignore_changes = [tags]
  }
}

data "aws_route53_zone" "app_zone" {
  count        = var.create_hosted_zone ? 0 : 1
  name         = var.hosted_zone
  private_zone = false
}

data "aws_route53_zone" "base_zone" {
  count        = var.enable_subdomain_delegation ? 1 : 0
  name         = var.base_hosted_zone
  private_zone = false
}

resource "aws_route53_record" "ns_record" {
  count   = var.enable_subdomain_delegation ? 1 : 0
  zone_id = data.aws_route53_zone.base_zone[0].zone_id
  name    = var.hosted_zone
  type    = "NS"
  ttl     = "300"

  records = [
    aws_route53_zone.main_zone[0].name_servers[0],
    aws_route53_zone.main_zone[0].name_servers[1],
    aws_route53_zone.main_zone[0].name_servers[2],
    aws_route53_zone.main_zone[0].name_servers[3]
  ]
}
