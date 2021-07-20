#!/bin/bash

# Sets environment variables
if grep -q 'mongodb://root:rootpassword@localhost:27017' /KavitaStats/appsettings.json
then
    sed -i "s,mongodb://root:rootpassword@localhost:27017,mongodb://${DB_USER}:${DB_PASS}@mongo:27017,g" /KavitaStats/appsettings.json
fi

if grep -q 'db_name_here' /KavitaStats/appsettings.json
then
    sed -i "s/db_name_here/${DB_NAME}/g" /KavitaStats/appsettings.json
fi

if grep -q 'api_key_here' /KavitaStats/appsettings.json
then
    sed -i "s/api_key_here/${API_KEY}/g" /KavitaStats/appsettings.json
fi

if grep -q 'allowed_hosts' /KavitaStats/appsettings.json
then
    sed -i "s/allowed_hosts/${ALLOWED_HOSTS}/g" /KavitaStats/appsettings.json
fi

dotnet Application.dll
