FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GetAdults/GetAdults.csproj GetAdults/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/
RUN dotnet restore GetAdults/GetAdults.csproj

COPY GetAdults/. GetAdults/
COPY SharedLibrary/. SharedLibrary/

WORKDIR /src/GetAdults
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY .oci/wallet /app/wallet
ENV TNS_ADMIN=/app/wallet

ENTRYPOINT ["dotnet", "GetAdults.dll"]
