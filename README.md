**AminesApi**

**Descripción**

AminesApi es una API RESTful que proporciona acceso a una base de datos de adultos y niños. La API permite realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) en la base de datos.

**Características**

* Utiliza Entity Framework Core para interactuar con la base de datos
* Define dos modelos de datos: `Adult` y `Child`
* Utiliza Swagger para documentar los endpoints y los modelos de datos
* Soporta operaciones CRUD para adultos y niños

**Endpoints**

* `GET /Adults`: devuelve una lista de adultos
* `GET /Adults/{id}`: devuelve un adulto específico por su ID
* `POST /Adults`: crea un nuevo adulto
* `PUT /Adults/{id}`: actualiza un adulto específico por su ID
* `DELETE /Adults/{id}`: elimina un adulto específico por su ID
* `GET /Children`: devuelve una lista de niños
* `GET /Children/{id}`: devuelve un niño específico por su ID
* `POST /Children`: crea un nuevo niño
* `PUT /Children/{id}`: actualiza un niño específico por su ID
* `DELETE /Children/{id}`: elimina un niño específico por su ID

**Tecnologías utilizadas**

* .NET Core
* Entity Framework Core
* Swagger
* C#

**Arquitectura**

* La API se divide en capas:
	+ Capa de datos: se encarga de interactuar con la base de datos utilizando Entity Framework Core
	+ Capa de negocio: se encarga de realizar las operaciones CRUD en la base de datos
	+ Capa de presentación: se encarga de manejar las solicitudes HTTP y devolver las respuestas

**Instalación y ejecución**

1. Clona el repositorio
2. Instala las dependencias utilizando `dotnet restore`
3. Ejecuta la API utilizando `dotnet run`

**Documentación**

La documentación de la API se puede encontrar en [Swagger](https://localhost:5001/swagger)

**Contribuciones**

Las contribuciones son bienvenidas. Por favor, envía un pull request con tus cambios.

**Licencia**

Este proyecto está licenciado bajo la licencia MIT.