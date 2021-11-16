FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY _output/ /KavitaStats
COPY entrypoint.sh /entrypoint.sh

WORKDIR /KavitaStats

EXPOSE 5001

ENTRYPOINT [ "/bin/bash" ]
CMD [ "/entrypoint.sh" ]