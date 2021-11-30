resource "aws_wafregional_web_acl" "waf_acl" {
  name        = "${var.application}-generic-owasp-acl"
  metric_name = "${var.metric_name}genericowaspacl"

  default_action {
    type = "ALLOW"
  }

  rule {
    action {
      type = "COUNT"
    }

    priority = 1
    rule_id  = aws_wafregional_rule.restrict_sizes.id
    type     = "REGULAR"
  }

  rule {
    action {
      type = "ALLOW"
    }

    priority = 2
    rule_id  = aws_wafregional_rule.detect_documents_and_messages_post.id
    type     = "REGULAR"
  }

  rule {
    action {
      type = "BLOCK"
    }

    priority = 3
    rule_id  = aws_wafregional_rule.mitigate_sqli.id
    type     = "REGULAR"
  }

  rule {
    action {
      type = "BLOCK"
    }

    priority = 4
    rule_id  = aws_wafregional_rule.mitigate_xss.id
    type     = "REGULAR"
  }

  rule {
    action {
      type = "BLOCK"
    }

    priority = 5
    rule_id  = aws_wafregional_rule.detect_rfi_lfi_traversal.id
    type     = "REGULAR"
  }

  rule {
    action {
      type = "BLOCK"
    }

    priority = 6
    rule_id  = aws_wafregional_rule.detect_php_insecure.id
    type     = "REGULAR"
  }

  rule {
    action {
      type = "BLOCK"
    }

    priority = 7
    rule_id  = aws_wafregional_rule.detect_ssi.id
    type     = "REGULAR"
  }
}
