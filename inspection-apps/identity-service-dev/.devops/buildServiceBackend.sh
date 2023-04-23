#!/bin/sh

set -eu

curl -o ${HOME}/.npmrc "${NPM_CONFIG_URL}"

cd IdentityPages
npm install --no-package-lock
npm run build -- --prod --deploy-url /identityservice/ --base-href /identityservice
cp -r dist/identity-pages/* ../${bamboo_ServiceName}/wwwroot
cd ..

dotnet publish -c Release --source ${NEXUS_WEB_URL}/repository/nuget-group/ 
mv ./${bamboo_ServiceName}/bin/Release/netcoreapp2.1/publish ./${bamboo_ServiceName}/bin/Release/netcoreapp2.1/WebPackage 
7z a ${bamboo_ServiceName}.${bamboo_planRepository_branch}.${bamboo_buildNumber}.zip "./${bamboo_ServiceName}/bin/Release/netcoreapp2.1/WebPackage*" "Database" "Certificates"
