#!/bin/bash

# Lista de proyectos creados
projectsList=("GetAdults" "GetChildren" "GetAdultById" "GetChildById" "AddMember" "PickAge" "BlobUpload" "AddAdult" "AddChildren")

for project in "${projectsList[@]}"
do
    case $project in 
        "GetAdults")
            cd "$project"
            dotnet add package Microsoft.EntityFrameworkCore
            dotnet add package Swashbuckle.AspNetCore
            dotnet add package Oracle.EntityFrameworkCore
            dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8
            cd ..
            ;;
        "GetChildren")
            cd "$project"
            dotnet add package Microsoft.EntityFrameworkCore
            dotnet add package Swashbuckle.AspNetCore
            dotnet add package Oracle.EntityFrameworkCore
            dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8
            cd ..
            ;;
        "GetAdultById")
            cd "$project"
            dotnet add package Microsoft.EntityFrameworkCore
            dotnet add package Swashbuckle.AspNetCore
            dotnet add package Oracle.EntityFrameworkCore
            dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8
            cd ..
            ;;
        "GetChildById")
            cd "$project"
            dotnet add package Microsoft.EntityFrameworkCore
            dotnet add package Swashbuckle.AspNetCore
            dotnet add package Oracle.EntityFrameworkCore
            dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8
            cd ..
            ;;
        "AddMember")
            cd "$project"
            dotnet add package Swashbuckle.AspNetCore
            dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8
            dotnet add package RabbitMQ.Client
            cd ..
            ;;
        "PickAge")
            cd "$project"
            cp ../.env.example .env 2>/dev/null || echo "No se encontró .env.example, créalo manualmente."
            cp ../AgeSelector.py .env 2>/dev/null || echo "No se encontró AgeSelector.py, créalo manualmente."
            cp ../Dockerfile .env 2>/dev/null || echo "No se encontró Dockerfile, créalo manualmente."
            cp ../requirements.txt .env 2>/dev/null || echo "No se encontró requirements.txt, créalo manualmente."
            cd ..
            ;;
        "BlobUpload")
            cd "$project"
            dotnet add package OCI.DotNetSDK.ObjectStorage
            cd ..
            ;;
        "AddAdult")
            rmdir "$project" 2>/dev/null || echo "Directorio $project no existe o no se pudo eliminar."
            dotnet new console -o "$project" --force
            cd "$project"
            dotnet add package RabbitMQ.Client
            dotnet add package Microsoft.EntityFrameworkCore
            dotnet add package Oracle.EntityFrameworkCore
            dotnet add package Microsoft.Extensions.Configuration
            dotnet add package Microsoft.Extensions.Configuration.FileExtensions
            dotnet add package Microsoft.Extensions.Configuration.Json
            cp ../Dockerfile . 2>/dev/null || echo "No se encontró Dockerfile, créalo manualmente."
            cd ..
            ;;
        "AddChildren")
            rmdir "$project" 2>/dev/null || echo "Directorio $project no existe o no se pudo eliminar."
            dotnet new console -o "$project" --force
            cd "$project"
            dotnet add package RabbitMQ.Client
            dotnet add package Microsoft.EntityFrameworkCore
            dotnet add package Oracle.EntityFrameworkCore
            dotnet add package Microsoft.Extensions.Configuration
            dotnet add package Microsoft.Extensions.Configuration.FileExtensions
            dotnet add package Microsoft.Extensions.Configuration.Json
            cp ../Dockerfile . 2>/dev/null || echo "No se encontró Dockerfile, créalo manualmente."
            cd ..
            ;;
    esac
done

echo "Paquetes instalados y configuraciones actualizadas para los 9 proyectos."