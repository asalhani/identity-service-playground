#!/bin/sh

set -eu

echo "${bamboo_npm_login}" >> ~/.npmrc

cd WebLibraries/dist/identity-ui
npm publish --registry ${NEXUS_WEB_URL}/repository/npm-private/

cd ../identity-guards
npm publish --registry ${NEXUS_WEB_URL}/repository/npm-private/
