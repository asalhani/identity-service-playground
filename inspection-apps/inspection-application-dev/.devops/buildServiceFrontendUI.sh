#!/bin/sh

set -eu
# TODO

cd InspectionWebApp
npm install --no-package-lock
ng build --prod

7z a ../InspectionApplication.${bamboo_planRepository_branch}.${bamboo_buildNumber}.zip "dist/InspectionWebApp*"

cd ..
tar -zcvf serviceDistFolder InspectionWebApp/dist/
