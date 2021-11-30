resource "aws_acm_certificate" "certificate" {
  domain_name               = var.domain_name
  subject_alternative_names = var.subject_alternative_names
  validation_method         = "DNS"

  tags = merge(
    var.default_tags,
    {
      "Name" = "${var.application}-certificate"
    },
  )

  lifecycle {
    create_before_destroy = true
  }
}

resource "aws_route53_record" "validation" {
  count = length(var.subject_alternative_names) + 1

  name            = tolist(aws_acm_certificate.certificate.domain_validation_options)[count.index].resource_record_name
  type            = tolist(aws_acm_certificate.certificate.domain_validation_options)[count.index].resource_record_type
  zone_id         = var.hosted_zone_id
  records         = [tolist(aws_acm_certificate.certificate.domain_validation_options)[count.index].resource_record_value]
  ttl             = var.validation_record_ttl
  allow_overwrite = true
}

resource "aws_acm_certificate_validation" "certificate" {
  certificate_arn = aws_acm_certificate.certificate.arn

  validation_record_fqdns = [
    aws_route53_record.validation[0].fqdn,
  ]
}