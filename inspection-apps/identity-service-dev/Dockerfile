FROM nexus.elm.sa/elm-core/dotnetcore:2.2-7z-16.02-node10.16.0-core0.3.8 AS build
WORKDIR /src
COPY . .

# Building service identity-pages angular application
ENV SASS_BINARY_SITE="https://nexus.elm.sa/repository/packages/sass/node-sass/releases/download"
WORKDIR IdentityPages
RUN npm config set registry https://nexus.elm.sa/repository/npm-group/ && \
npm install --no-package-lock && \
npm run build -- --prod --deploy-url /identityservice/ --base-href /identityservice && \
cp -r dist/identity-pages/* ../IdentityService/wwwroot

# Building service back-end .NET
WORKDIR "/src/IdentityService"
RUN dotnet publish "IdentityService.csproj" -c Release -o /app --source https://nexus.elm.sa/repository/nuget-group/

FROM nexus.elm.sa/elm-core/dotnet:2.1-aspnetcore-runtime-gdi AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
COPY ["Certificates", "Certificates/"]
COPY ["Database", "Database/"]
ENTRYPOINT ["dotnet", "IdentityService.dll","-ci","./Database"]

