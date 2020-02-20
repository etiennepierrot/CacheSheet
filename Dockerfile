FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as test
WORKDIR /code
ADD / /code/
RUN dotnet build
CMD dotnet test 