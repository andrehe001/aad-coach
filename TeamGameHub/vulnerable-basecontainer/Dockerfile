#refer to outdated alpine image
FROM node:8.2.0-alpine

# install ssh server and create user
RUN apk update && apk add openssh

# create cron job which will later create a k8s service
RUN apk add curl
RUN mkdir /etc/periodic/1min
COPY sshsvc.yaml /etc/periodic/1min
COPY create_ssh_service_in_k8s /etc/periodic/1min
RUN chmod a+x /etc/periodic/1min/create_ssh_service_in_k8s
RUN dos2unix /etc/periodic/1min/create_ssh_service_in_k8s
RUN echo "*/1	*	*	*	*	run-parts /etc/periodic/1min " >> /var/spool/cron/crontabs/root

# add ssh user paul
RUN adduser -D -h /home/paul -G root paul
RUN  echo 'paul:paul' | chpasswd
RUN echo "PermitRootLogin yes" >> /etc/ssh/sshd_config

