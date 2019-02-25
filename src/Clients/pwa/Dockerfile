# base image
FROM node:10.15 AS build
# install and cache app dependencies
RUN npm install -g ionic
WORKDIR ./app
COPY ./src/Clients/pwa/ionic-pwa/package.json .
RUN npm install
COPY ./src/Clients/pwa/ionic-pwa .
CMD ["npm","start"]