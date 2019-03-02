# base image
FROM node:10.15 AS build
ARG APPDIR=./src/Clients/pwa/ionic-pwa
# install and cache app dependencies
RUN npm install -g ionic
WORKDIR /usr/src/app
COPY ${APPDIR}/package.json .
RUN npm install
CMD ["npm","start"]