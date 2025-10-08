FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AddAdult/AddAdult.csproj AddAdult/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/
RUN dotnet restore AddAdult/AddAdult.csproj

COPY AddAdult/. AddAdult/
COPY SharedLibrary/. SharedLibrary/

WORKDIR /src/AddAdult
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY .oci/wallet /app/wallet
ENV TNS_ADMIN=/app/wallet

ENTRYPOINT ["dotnet", "AddAdult.dll"]
