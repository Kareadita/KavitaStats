#FROM mcr.microsoft.com/dotnet/sdk:5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY Release/ /KavitaStats

WORKDIR /KavitaStats

EXPOSE 80
EXPOSE 443

CMD [ "dotnet", "Application.dll" ]