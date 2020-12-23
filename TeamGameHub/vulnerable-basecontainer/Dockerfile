#refer to outdated alpine image
FROM node:8.2.0-alpine

# install ssh server and create user
RUN apk update && apk add openssh
RUN adduser -D -h /home/paul -G root paul
RUN  echo 'paul:paul' | chpasswd
RUN echo "PermitRootLogin yes" >> /etc/ssh/sshd_config
