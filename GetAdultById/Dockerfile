FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GetAdultById/GetAdultById.csproj GetAdultById/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/
RUN dotnet restore GetAdultById/GetAdultById.csproj

COPY GetAdultById/. GetAdultById/
COPY SharedLibrary/. SharedLibrary/

WORKDIR /src/GetAdultById
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY .oci/wallet /app/wallet
ENV TNS_ADMIN=/app/wallet

ENTRYPOINT ["dotnet", "GetAdultById.dll"]
