# AminesApi

## Descripción

AminesApi es una API RESTful que proporciona acceso a una base de datos de adultos y niños, permitiendo realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) de manera sencilla y escalable. Esta API ha evolucionado para integrar almacenamiento en la nube y una base de datos Oracle, convirtiéndose en una herramienta práctica para gestionar información de personas.

## Características

- Utiliza Entity Framework Core para interactuar con una base de datos Oracle Autonomous Database (ADB).
- Define dos modelos de datos: `Adult` y `Child` con propiedades como `Name`, `Lastname`, `BirthYear`, y `ImageURL`.
- Incluye Swagger para documentar endpoints y modelos de datos de forma interactiva.
- Soporta operaciones CRUD completas para adultos y niños.
- Permite subir imágenes de usuarios a OCI Object Storage y almacenar las URLs en la base de datos.
- Arquitectura modular en capas para facilitar el mantenimiento y la escalabilidad.

## Endpoints

- `GET /Adults`: Devuelve una lista de adultos.
- `GET /Adults/{id}`: Devuelve un adulto específico por su ID.
- `POST /Add/Member`: Crea un nuevo adulto o niño (con soporte para subir imágenes).
- `PUT /Adults/{id}`: Actualiza un adulto específico por su ID.
- `DELETE /Adults/{id}`: Elimina un adulto específico por su ID.
- `GET /Children`: Devuelve una lista de niños.
- `GET /Children/{id}`: Devuelve un niño específico por su ID.
- `POST /Add/Member`: Crea un nuevo adulto o niño (con soporte para subir imágenes).
- `PUT /Children/{id}`: Actualiza un niño específico por su ID.
- `DELETE /Children/{id}`: Elimina un niño específico por su ID.

*Nota*: El endpoint `POST /Add/Member` reemplaza los anteriores `POST /Adults` y `POST /Children` para unificar la creación con subida de imágenes.

## Tecnologías Utilizadas

- **.NET Core 8**: Framework principal para la API.
- **Entity Framework Core**: ORM para interactuar con Oracle ADB.
- **Oracle.ManagedDataAccess.Client**: Proveedor para conectar con Oracle ADB.
- **Swagger/OpenAPI**: Documentación interactiva de la API.
- **OCI Object Storage SDK**: Para subir y gestionar imágenes en la nube.
- **C#**: Lenguaje de programación principal.

## Arquitectura

La API se divide en capas para una mejor organización:

- **Capa de datos**: Interactúa con Oracle ADB usando Entity Framework Core, con un `DataContext` configurado para mapear entidades a tablas en el esquema `INGRID`.
- **Capa de negocio**: Realiza las operaciones CRUD y gestiona la lógica de subida de imágenes a OCI.
- **Capa de presentación**: Maneja las solicitudes HTTP, integra Swagger, y devuelve respuestas en formato JSON.

## Instalación y Ejecución

1. Clona el repositorio:

https://github.com/IngridBianchi/AminesApi

2. Instala las dependencias:

cd AminesApi/
dotnet restore

3. Configura las credenciales de Oracle ADB y OCI:
- Copia el archivo `appsettings.Development.json` y ajusta las claves (`ConnectionStrings:DefaultConnection`, `OCI:ConfigFilePath`, `OCI:BucketName`, etc.) con tus valores.

4. Ejecuta la API:

dotnet run


## Documentación

La documentación de la API está disponible en [Swagger](http://localhost:5224/swagger) una vez que la aplicación esté en ejecución.

## Contribuciones

¡Las contribuciones son bienvenidas! Si tenés ideas (como nuevos endpoints, autenticación, o mejoras en el almacenamiento), enviá un pull request con tus cambios. ¡Colaboremos para hacerla aún mejor!

## Licencia

Este proyecto está licenciado bajo la licencia MIT.

## Actualizaciones Recientes (20/08/2025)

- Integración exitosa con Oracle Autonomous Database, resolviendo errores de cuota (ORA-01950) al asignar cuota ilimitada al usuario `INGRID` en el tablespace `DATA`.
- Implementación de la subida de imágenes a OCI Object Storage, con URLs almacenadas en la base de datos.
- Optimización del mapeo en `DataContext.cs` para alinear las entidades `Adult` y `Child` con las tablas `ADULT` y `CHILD` en el esquema `INGRID`.
- Pruebas completas con Swagger y verificaciones manuales en Database Actions.

