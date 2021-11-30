resource "aws_wafregional_rule" "detect_php_insecure" {
  name        = "${var.application}-generic-detect-php-insecure"
  metric_name = "${var.metric_name}genericdetectphpinsecure"

  predicate {
    data_id = aws_wafregional_byte_match_set.match_php_insecure_uri.id
    negated = false
    type    = "ByteMatch"
  }

  predicate {
    data_id = aws_wafregional_byte_match_set.match_php_insecure_var_refs.id
    negated = false
    type    = "ByteMatch"
  }
}

resource "aws_wafregional_rule" "detect_rfi_lfi_traversal" {
  name        = "${var.application}-generic-detect-rfi-lfi-traversal"
  metric_name = "${var.metric_name}genericdetectrfilfitraversal"

  predicate {
    data_id = aws_wafregional_byte_match_set.match_rfi_lfi_traversal.id
    negated = false
    type    = "ByteMatch"
  }
}

resource "aws_wafregional_rule" "detect_ssi" {
  name        = "${var.application}-generic-detect-ssi"
  metric_name = "${var.metric_name}genericdetectssi"

  predicate {
    data_id = aws_wafregional_byte_match_set.match_ssi.id
    negated = false
    type    = "ByteMatch"
  }
}

resource "aws_wafregional_rule" "mitigate_sqli" {
  name        = "${var.application}-generic-mitigate-sqli"
  metric_name = "${var.metric_name}genericmitigatesqli"

  predicate {
    data_id = aws_wafregional_sql_injection_match_set.sql_injection_match_set.id
    negated = false
    type    = "SqlInjectionMatch"
  }
}

resource "aws_wafregional_rule" "detect_documents_and_messages_post" {
  name        = "${var.application}-generic-detect-documents-and-messages-post"
  metric_name = "${var.metric_name}genericdetectdocmessagespost"

  predicate {
    data_id = aws_wafregional_byte_match_set.match_csrf_method.id
    negated = false
    type    = "ByteMatch"
  }

  predicate {
    data_id = aws_wafregional_byte_match_set.match_referer_document_messages.id
    negated = false
    type    = "ByteMatch"
  }
}


resource "aws_wafregional_rule" "mitigate_xss" {
  name        = "${var.application}-generic-mitigate-xss"
  metric_name = "${var.metric_name}genericmitigatexss"

  predicate {
    data_id = aws_wafregional_xss_match_set.xss_match_set.id
    negated = false
    type    = "XssMatch"
  }
}

resource "aws_wafregional_rule" "restrict_sizes" {
  name        = "${var.application}-generic-restrict-sizes"
  metric_name = "${var.metric_name}genericrestrictsizes"

  predicate {
    data_id = aws_wafregional_size_constraint_set.size_restrictions.id
    negated = false
    type    = "SizeConstraint"
  }
}

