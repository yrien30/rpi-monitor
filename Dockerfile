ARG BUILD_FOR
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS amd64
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal-arm64v8 AS arm64

FROM ${BUILD_FOR} AS final
# we need to add the raspberrypi GPG key which uses
# "apt-key adv" and this in return requires gnupg
RUN apt update && apt install -y gnupg
RUN \
  # Update system
  echo "deb http://ports.ubuntu.com/ubuntu-ports focal main" >> /etc/apt/sources.list.d/ubuntu-main.list && \
  apt-key adv --recv-keys --keyserver keyserver.ubuntu.com 3B4FE6ACC0B21F32 40976EAF437D05B5 && \
  apt-get update && \
  apt-get upgrade -y

  
RUN apt-get install -y libraspberrypi-bin
WORKDIR /deploy
COPY deploy/. ./
LABEL org.opencontainers.image.source https://github.com/yrien30/rpi-monitor
ENTRYPOINT ["dotnet", "rpi-monitor.dll"]
