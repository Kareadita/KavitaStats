# KavitaStats
Stats API server for Kavita (not for public use)

If you are developing and testing, you can run the image with

`docker run -p 5001:5001 -v ${PWD}/config:/app/config -d jvmilazz0/kavitastats:latest`

In order to use this you will have to compile your own modified Kavita instance, since the Stats server host is hard coded in public releases.
