ARG BUILD_FOR
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS amd64
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal-arm64v8 AS arm64

FROM ${BUILD_FOR} AS final
RUN \
  # Update system
  echo "deb http://archive.raspberrypi.org/debian/ jessie main" >> /etc/apt/sources.list.d/raspberrypi.list && \
  apt-key adv --keyserver keyserver.ubuntu.com --recv 82B129927FA3303E && \
  apt-get update && \
  apt-get upgrade -y
RUN apt-get install -y libraspberrypi-bin
WORKDIR /deploy
COPY deploy/. ./
LABEL org.opencontainers.image.source https://github.com/yrien30/rpi-monitor
ENTRYPOINT ["dotnet", "rpi-monitor.dll"]
