FROM ubuntu:focal AS copytask

ARG TARGETPLATFORM

#Moves the files over
RUN mkdir /files
COPY _output/*.tar.gz /files/
COPY copy_runtime.sh /copy_runtime.sh
RUN /copy_runtime.sh

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 (Joe: I removed this as it was failing the build, using what we have for Kavita)
FROM ubuntu:focal

COPY --from=copytask /KavitaStats /app

COPY entrypoint.sh /entrypoint.sh

#Creates a temporary save of the config file
RUN cp /app/config/appsettings.json /tmp/appsettings.json

RUN apt-get update && \
    apt-get install -y curl nano rsync && \
    rm -rf /var/lib/apt/lists/*

EXPOSE 5001

WORKDIR /app

ENTRYPOINT [ "/bin/bash" ]
CMD [ "/entrypoint.sh" ]