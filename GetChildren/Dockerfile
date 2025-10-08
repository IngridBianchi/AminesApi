FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GetChildren/GetChildren.csproj GetChildren/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/
RUN dotnet restore GetChildren/GetChildren.csproj

COPY GetChildren/. GetChildren/
COPY SharedLibrary/. SharedLibrary/

WORKDIR /src/GetChildren
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY .oci/wallet /app/wallet
ENV TNS_ADMIN=/app/wallet

ENTRYPOINT ["dotnet", "GetChildren.dll"]
