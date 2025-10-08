FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AddChildren/AddChildren.csproj AddChildren/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/
RUN dotnet restore AddChildren/AddChildren.csproj

COPY AddChildren/. AddChildren/
COPY SharedLibrary/. SharedLibrary/

WORKDIR /src/AddChildren
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY .oci/wallet /app/wallet
ENV TNS_ADMIN=/app/wallet

ENTRYPOINT ["dotnet", "AddChildren.dll"]



