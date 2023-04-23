FROM nexus.elm.sa/nginx:1.15
## Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*
## From 'builder' stage copy over the artifacts in dist folder to default nginx public folder
COPY InspectionWebApp/dist/InspectionWebApp /usr/share/nginx/html
# EXPOSE 9080
CMD ["nginx", "-g", "daemon off;"]
