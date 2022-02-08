#!/bin/bash

#Copies the correct version of Kavita into the image

set -xv

if [ "$TARGETPLATFORM" == "linux/amd64" ]
then
	tar xf /files/kavitastats-linux-x64.tar.gz -C /
elif [ "$TARGETPLATFORM" == "linux/arm/v7" ]
then
	tar xf /files/kavitastats-linux-arm.tar.gz -C /
elif [ "$TARGETPLATFORM" == "linux/arm64" ]
then
	tar xf /files/kavitastats-linux-arm64.tar.gz -C /
fi