FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs
RUN npm install @angular/cli -g
WORKDIR /app/ClientApp
COPY ./ClientApp/package.json .
RUN npm install
RUN npm rebuild node-sass
EXPOSE 80
EXPOSE 4200

