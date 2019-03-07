FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet build --no-restore -c Release -o /release

FROM node:10.15 as node-build
WORKDIR /src
COPY ./ClientApp/package.json .
RUN npm install
RUN npm rebuild node-sass
COPY ./ClientApp .
RUN npm run build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /release


FROM base AS final
WORKDIR /app
COPY --from=publish /release .
COPY --from=node-build /src/www ./ClientApp/dist
ENTRYPOINT ["dotnet", "IonicPWA.dll"]

