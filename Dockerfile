FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /code
ADD / /code/

RUN dotnet build