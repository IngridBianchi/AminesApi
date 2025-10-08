FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo los archivos de proyecto
COPY AddMember/AddMember.csproj AddMember/
COPY SharedLibrary/SharedLibrary.csproj SharedLibrary/

# Restaura dependencias
RUN dotnet restore AddMember/AddMember.csproj

# Copia solo el código fuente necesario
COPY AddMember/. AddMember/
COPY SharedLibrary/. SharedLibrary/

WORKDIR /src/AddMember
RUN rm -rf ./bin ./obj ../SharedLibrary/bin ../SharedLibrary/obj
RUN dotnet build AddMember.csproj -c Release -o /app/build
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AddMember.dll"]