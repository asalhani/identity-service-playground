#!/bin/sh

set -eu

case ${bamboo_planRepository_branch} in
  dev)
      echo "Branch (${bamboo_planRepository_branch})"
      echo "Building a new docker image with tag (latest)"
      tar xvzf serviceDistFolder -C .
      docker build -t nexus.elm.sa/elm-inspection/inspectionwebapplication:latest .
      docker push nexus.elm.sa/elm-inspection/inspectionwebapplication:latest
      ;;
  qa)
      echo "Branch (${bamboo_planRepository_branch})"
      echo "Tagging latest docker image with tag (qa-latest)"
      docker pull nexus.elm.sa/elm-inspection/inspectionwebapplication:latest
      docker tag nexus.elm.sa/elm-inspection/inspectionwebapplication:latest nexus.elm.sa/elm-inspection/inspectionwebapplication:qa-latest
      docker push nexus.elm.sa/elm-inspection/inspectionwebapplication:qa-latest
      ;;
  *)
      echo "Sorry branch is not [dev|qa], Ignoring branch ${bamboo_planRepository_branch}"
      ;;
esac

cat > ${bamboo_ServiceName}.info <<EOF
DOCKER_REGISTRY_PATH=elm-inspection/inspectionwebapplication
PLAN_BUILD_NUMBER=${bamboo_buildNumber}
GIT_REPOSITORY_BRANCH=${bamboo_planRepository_branch}
GIT_REPOSITORY_URL=${bamboo_planRepository_repositoryUrl}
GIT_REPOSITORY_HASH_COMMIT=${bamboo_planRepository_revision}
EOF
