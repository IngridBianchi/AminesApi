# AminesApi

## Descripción

AminesApi es una API RESTful diseñada para gestionar información de usuarios clasificados como adultos o niños según su año de nacimiento (>=18 adulto). Permite operaciones CRUD (Crear, Leer, Actualizar, Eliminar) de manera escalable y ha evolucionado a una arquitectura de microservicios. Integra Oracle Autonomous Database (ADB) para persistencia, OCI Object Storage para imágenes, y RabbitMQ para mensajería, convirtiéndose en una herramienta práctica para manejar datos de personas.

## Características

- Utiliza Entity Framework Core para interactuar con Oracle ADB.
- Define modelos Adult y Child con propiedades como Name, Lastname, BirthYear, y ImageURL.
- Incluye Swagger para documentación interactiva de endpoints y modelos.
- Soporta operaciones CRUD completas para adultos y niños.
- Permite subir imágenes a OCI Object Storage y almacenar URLs en ADB.
- Implementa una arquitectura de microservicios con RabbitMQ para procesar mensajes (e.g., PickAge clasifica usuarios).
- Usa una librería compartida (SharedLibrary) para centralizar modelos y lógica de datos.
- Arquitectura modular en capas para mantenimiento y escalabilidad.

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

- .NET Core 8: Framework principal para la API y microservicios.
- Entity Framework Core 9.0.8: ORM para Oracle ADB.
- Oracle.EntityFrameworkCore 9.23.90: Proveedor específico para Oracle.
- RabbitMQ.Client 7.1.2: Mensajería con exchange usuarios_topic y routing_keys adult/child.
- OCI.DotNetSDK.Common/Objectstorage 117.0.0: Gestión de imágenes en la nube.
- Python 3.12 con pika 1.3.2: Microservicio PickAge para clasificación de usuarios.
- Swashbuckle.AspNetCore 9.0.3: Documentación Swagger.
- Microsoft.AspNetCore.OpenApi 8.0.8: Soporte OpenAPI.
- C#: Lenguaje principal.
- GitHub: Control de versiones con .gitignore (ignora bin/obj, appsettings.json, venv).

## Arquitectura

La API se organiza en capas y microservicios:
- Capa de datos: SharedLibrary/Data/Datacontext.cs interactúa con Oracle ADB, mapeando Adult y Child a tablas en el esquema INGRID.
- Capa de negocio: Lógica CRUD y subida de imágenes a OCI, distribuida en microservicios como AddAdult.
- Capa de presentación: Maneja solicitudes HTTP, integra Swagger, y devuelve JSON.

Microservicios:
- AddMember: Envía mensajes a RabbitMQ.
- PickAge (Python): Clasifica usuarios por edad.
- AddAdult/AddChildren: Consume mensajes y guardan en ADB.

## Instalación y Ejecución

1- Clona el repositorio:

git clone https://github.com/IngridBianchi/AminesApi.git
cd AminesApi


2- Instala las dependencias:

dotnet restore
cd PickAge
pip install -r requirements.txt


3- Configura las credenciales:

Copia appsettings.Development.json y ajusta:

ConnectionStrings:DefaultConnection: Cadena para Oracle ADB (e.g., User Id=INGRID;Password=tu_password;Data Source=tu_adb).

RabbitMQ:Host: localhost (puerto 5672, credenciales guest/guest).

OCI:ConfigFilePath, OCI:BucketName: Rutas y nombres de OCI.

Configura PickAge/.env con RABBITMQ_HOST=localhost, RABBITMQ_USER=guest, RABBITMQ_PASSWORD=guest.


4- Ejecuta los microservicios:

API y microservicios .NET:

cd AddAdult
dotnet run
cd ../AddChildren
dotnet run
cd ../AddMember
dotnet run


PickAge (Python):

cd PickAge
python pick_age.py



Verifica RabbitMQ local:

sudo rabbitmqctl status


## Documentación

Accede a la documentación en Swagger al ejecutar la API.

## Contribuciones

¡Las contribuciones son bienvenidas! Si tenés ideas (como nuevos endpoints, autenticación, o mejoras en el almacenamiento), enviá un pull request con tus cambios. ¡Colaboremos para hacerla aún mejor!

## Licencia

Este proyecto está licenciado bajo la licencia MIT.

## Actualizaciones Recientes (20/08/2025)

- Integración exitosa con Oracle Autonomous Database, resolviendo errores de cuota (ORA-01950) al asignar cuota ilimitada al usuario `INGRID` en el tablespace `DATA`.
- Implementación de la subida de imágenes a OCI Object Storage, con URLs almacenadas en la base de datos.
- Optimización del mapeo en `DataContext.cs` para alinear las entidades `Adult` y `Child` con las tablas `ADULT` y `CHILD` en el esquema `INGRID`.
- Pruebas completas con Swagger y verificaciones manuales en Database Actions.

## Actualizaciones Recientes (11/09/2025)

- Evolución a arquitectura de microservicios con AddMember, PickAge, AddAdult, y AddChildren.
- Integración de RabbitMQ local (puerto 5672) para mensajería, con PickAge clasificando usuarios (e.g., 1991 como adulto).
- Resolución de errores de compilación (CS0234, CS0246, CS1061, CS0436) al mover Data y Models a SharedLibrary.
- Corrección de namespaces en Datacontext.cs a SharedLibrary.Models.
- Restauración forzada de paquetes (dotnet restore --force) para RabbitMQ.Client y Oracle.EntityFrameworkCore.
- Subida inicial a GitHub con .gitignore configurado.

