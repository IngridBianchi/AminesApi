FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BlobUpload/BlobUpload.csproj BlobUpload/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/

RUN dotnet restore BlobUpload/BlobUpload.csproj

COPY BlobUpload/ BlobUpload/
COPY SharedLibrary/ SharedLibrary/

COPY BlobUpload/appsettings.json BlobUpload/
COPY BlobUpload/appsettings.Development.json BlobUpload/

RUN dotnet publish BlobUpload/BlobUpload.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
COPY .oci/wallet /app/wallet
ENV TNS_ADMIN=/app/wallet

ENTRYPOINT ["dotnet", "BlobUpload.dll"]
