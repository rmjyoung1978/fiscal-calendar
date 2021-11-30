#!/bin/bash
set -euo pipefail

cd infra

# download plan to artifacts
mkdir package
buildkite-agent artifact download "deploy-api-package.zip" "package"

# run terraform deploy
make build
make deploy

# cleanup artifacts
buildkite-agent artifact upload "${ENV}.${AWS_DEFAULT_REGION}.plan"
rm -f "${ENV}.${AWS_DEFAULT_REGION}.plan"
