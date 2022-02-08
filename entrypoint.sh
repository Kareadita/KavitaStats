#!/bin/bash

#Checks if the appsettings.json already exists in bind mount
if test -f "/app/config/appsettings.json"
then
	echo "appsettings.json exists, skipping..."
else
	cp /tmp/appsettings.json /app/config/appsettings.json
fi

chmod +x /app/KavitaStats

./KavitaStats
