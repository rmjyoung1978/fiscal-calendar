#!/bin/bash
set -euo pipefail

cd src

make build

cd artifacts

# publish the zip
buildkite-agent artifact upload "deploy-api-package.zip"

rm -f  "deploy-api-package.zip"
