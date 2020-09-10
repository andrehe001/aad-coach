FROM node:lts-alpine AS build-env
WORKDIR /usr/src/app
COPY babel.config.js ./
COPY package.json ./
COPY package-lock.json ./
RUN npm install
COPY ./public ./public
COPY ./src ./src
RUN npm run build

FROM nginx
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build-env /usr/src/app/dist/ /usr/share/nginx/html/
COPY ./nginx.conf /etc/nginx/nginx.conf

ENTRYPOINT ["nginx", "-g", "daemon off;"]