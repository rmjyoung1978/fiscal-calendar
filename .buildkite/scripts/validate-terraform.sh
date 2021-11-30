#!/bin/bash
set -euo pipefail
cd infra

# download plan to artifacts
mkdir package
buildkite-agent artifact download "deploy-api-package.zip" "package"

# run terraform test
make test