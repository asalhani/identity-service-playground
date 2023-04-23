#!/bin/sh

set -eu

cd WebLibraries
npm install --no-package-lock
npm run build -- identity-ui --prod
npm run build -- identity-guards --prod
