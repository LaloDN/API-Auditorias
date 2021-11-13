**Contenido**

[TOCM]

[TOC]

### Dependencias
Esta API Rest esta construida con los siguientes paquetes/herramientas:
- .Net Core (3.1)
- Microsoft.AspNetCore.Authentication.JwtBearer (3.1.8)
- Microsoft.EntityFrameworkCore (3.1.7)
- Microsoft.EntityFrameworkCore.Tools(3.1.7)
- Pomelo.EntityFrameworkCore.MySql(3.1.2)

### Estructura del proyecto
Este proyecto funciona con el patron de diseño **repository**.
#### Elementos del proyecto
- Controladores: Son clases que heredan de *ControllerBase*, en dónde tenemos la colección de nuestros endpoints.
- Modelos: Son las clases que contienen los atributos que posteriormente serán convertidas a tablas y campos.
- IRepository e IService: Son interfaces las cuáles nos ayudan a que los servicios y los repositorios sigan un contrato y esten atados a implementar los prototipos de los métodos dentro de nuestras interfaces (Nota: *Ambas inerfaces van a tener los mismos métodos*)
- Services: En estas clases esta la lógica del negocio, implementan la interfaz IService que le corresponda a cada clase, sirven como middleware para que el controlador y los repositorios se comuniquen entre si.
- Repositories: Contiene una instancia del contexto de la base de datos, con la cual realizan operaciones.
- Context: Es una clase que hereda de DbContext, aquí establecemos las instancias de DbSet (tablas en la base de datos), le pasamos la clase que vamos a convertir en una entidad así como el nombre que va a tener en la base de datos.
- DTO: Son objetos auxiliares que nos ayudan a ser peticiones en los endpoints a los cuales no tenemos un modelo que cumpla con la estructura que necesita el body de las peticiones
- Utils: Es la carpeta con las clases que nos ayudan a realizar procesos en nuestra aplicación, como encriptar contraseñas u obtener los claims de un JWT.
#### Diagrama secuencial del patrón de dsieño

```seq
Frontend->Controlador:Petición
Controlador->Service: Puente hacía repository
Service->Repository: Operación con la base de datos
Repository->Service: Devuelve una tarea
Service->Controlador: Puente hacía controlador
Controlador->Frontend: Respuesta de la petición
```
#### Sistema de carpetas
La vista principal del proyecto es la siguiente <br/>  

![vista de carpetas](/imagenes/carpetas.png)

#### Distribución de archivos
+ Controllers: Contiene los controladores del proyecto.
+ Domain:
    + IServices: Contiene las interfaces que van a implementar los service.
    + IRepositories: Contiene las interfaces que van a implementar los repository.
    + Models: Son las clases que van a ser convertidas en entidades a la base de datos.
+ DTO: Son los objetos auxiliares los cuales, los cuales tienen atritbutos que se adaptan a los parámetros del body en la petición.
+ Migrations: Aquí se almacenan los procesos de las migraciones que realizemos.
+ Persistence: Es la carpeta en la que tenemos archivos relacionados a la BD.
    * Context: Es el contexto de la base de datos, en dónde definimos las entidades que vamos a agregar.
    * Repositories: Los repositorios que hacen peticiones directamente a la base de datos.
+ Services: La carpeta de nuestra lógica de negocio que funciona como middleware entre controladores y repositorios.
+ Utils: Aquí se guardan todos las clases que nos puedan utilizar a lo largo del proyecto que nos ayudan a realizar diferentes operaciones. 

#### Proceso de creación de un nuevo componente.
1. Se crea una interfaz para el servicio y el repositorio dentro de la carpeta correspondiente (Nota: Al momento de crearse, las interfaces deben ser públicas).
2. Ahora se tiene que crear una clase repository, con una instancia *readonly* de la base de datos para hacer las operaciones correspondientes, esta clase debe de implementar a su respectivo IRepository.
3. Se crea una clase services que va a tener una instancia *readonly* del repositorio, esta clase tiene que implementar a su respectivo IService.
4. Por último, se crea un controlador que herede de *ControllerBase*, este controlador también va a tener una instancia *readonly* del service al que le corresponde.
5. Para poder utilizar los servicios en el controlador, necesitamos ir al archivo *Startup.cs*, buscar el método *ConfigureServices* e y agregar los servicios y el repositorio de la siguiente manera:

```cs
        public void ConfigureServices(IServiceCollection services)
        {

            //Servicos
            services.AddScoped<InterfazService, MiService>();

            //Repositorios
            services.AddScoped<InterfazRepository,MiRepository>();
		}
````
> NOTA: Si algún servicio o repositorio no implementa una interfaz, el código marcará error.

Con esto ya podemos trabajar en nuestro nuevo controlador y programando los endpoints.

#### Configurando otros service
Dentro de este método service ya tenemos nuestra base de datos configurada, así como el CORS para recibir peticiones del frontend, si se desea agregar algún otro servicio en el futuro, se tiene que inyectar en este método para que la aplicación lo pueda utilizar.
### Controladores y endpoints

------------


#### UsuarioController
Dentro de este controlador vamos a tener los endpoints relacionados con las operaciones del usuario, como la alta y baja o cambio de contraseñas
##### POST
*Nivel de acceso: Público* \n
*Requiere token: No* \n
*Descripción: Agrega un usuario a la base de datos* \n
*Parámetros del body:  Modelo Usuario* \n
![Endpoint agregar usuario](/imagenes/1.png)
##### GET
*Nivel de acceso:* Solo usuarios RH
*Requiere token: *Si
*Descripción:* Devuelve la lista de usuarios en la base de datos
*Parámetros del body*: Ninguno
![Endpoint lista de usuarios](/imagenes/2.png)
##### PUT
*Ruta de acceso:* /CambiarPassword
*Nivel de acceso*: Usuarios dentro de la base
*Requiere token:*Si
*Descripción:*Cambia la contraseña de un usuario en la base
*Parámetros del body:*Recibe un modelo cambiarPasswordDTO con la contraseña actual y la nueva
![Endpoint para cambiar password](/imagenes/3.png)
##### DELETE
*Nivel de acceso*: Solo usuarios RH
*Requiere token:* Si
*Descripción:* Elimina un usuario de la base de datos con su id
*Parámetros de la petición:* Al final del link de la petición se agrega un /id=(id usuario a borrar)
![Endpoint para borrar usuarios](/imagenes/4.png)

------------

#### LoginController
Contiene un único endpoint que se utiliza para hacer el login al sistema.

##### POST
*Nivel de acceso:* Público
*Requiere token:* No
*Descripción:*  Verifica si el usuario a ingresar es válido y le regresa un token
*Parámetros del body:* Recibe un objeto LoginDTO con la información de inicio de sesión del usuario
![Endpoint para hacer login](/imagenes/5.png)
------------

#### DepartamentoController
Contiene un único endpoint que nos sirve para obtener la lista de departamentos dentro de la base.
##### GET
*Nivel de acceso*: Solo usuarios RH
*Requiere token:* Si
*Descripción:* Devuelve la lista de los departamentos registrados en la base de datos
*Parámetros del body:* Ninguno
![Endpoint para obtener los departamentos](/imagenes/6.png)

------------

#### PreguntaController
Tiene endpoints para agregar y traer preguntas de la base de datos.
##### POST
*Nivel de acceso*: Solo usuarios RH
*Requiere token:* Si
*Descripción:* Agrega una pregunta a la base de datos
*Parámetros del body:* Modelo Pregunta
![Endpoint para agergar preguntas](/imagenes/7.png)

##### GET
*Nivel de acceso*: Solo usuarios RH
*Requiere token:* Si
*Descripción:* Devuelve la lista de preguntas que tenemos disponibles en la base de datos
*Parámetros del body:* Ninguno
![Endpoint para obtener el catálogo de preguntas](/imagenes/8.png)

------------

#### AuditoriaController
Aquí estan todos los endpoints que nos van a ayudar para el proceso de la adutoría
##### POST
*Nivel de acceso*: Solo usuarios RH
*Requiere token:* Si
*Descripción:* Crea una auditoria vacía y devuelve el id (int) con el que fue registrada en la base de datos 
*Parámetros del body:* Modelo Auditoria
![Endpoint para crear una auditoria](/imagenes/9.png)
##### POST
*Ruta de accceso:* /CrearAuditoria 
*Nivel de acceso*: Solo usuarios RH
*Requiere token:* Si
*Descripción:* Asigna un grupo de respuestas a una auditoría
*Parámetros del body:* Lista de modelos PreguntaRespuesta
![Endpoint para asignar preguntas](/imagenes/10.png)
#### GET
*Nivel de acceso*: Usuarios dentro del sistema
*Requiere token:* Si
*Descripción:* Obtiene la lista de las auditorias creadas
*Parámetros del body:* Ninguno
![Endpoint para obtener la lista de aduitorías](/imagenes/11.png)
#### GET
*Ruta de acceso:* /VerAuditoria
*Nivel de acceso*: Usuarios dentro del sistema
*Requiere token:* Si
*Descripción:* Obtiene la lista de preguntas que fueron asignadas a una auditoría
*Parámetros del body:* Objeto IdAuditoria con el id de la auditoria que queremos ver.
![Endpoint para obtener la lista de preguntas de una auditoría](/imagenes/12.png)
#### PUT
*Ruta de acceso:* /CambiarEstadoEP
*Nivel de acceso*: Solo usuarios auditores
*Requiere token:* Si
*Descripción:* Se invoca cuándo se va a acceder a una auditoria, pregunta por su estado, si la auditoria esta "En Proceso", devuelve un -1, si la auditoría no está en proceso, cambia su estado a "En Proceso" y devuelve un 1, si el usuario no tiene permiso devuelve un 0.
*Parámetros del body:* Objeto IdAuditoria con el id de la auditoria a aplicar.
![Endpoint cambiar estado primero](/imagenes/13.png)
![Endpoint cambiar estado cuando esta en proceso](/imagenes/14.png)
#### PUT
*Ruta de acceso:* /CambiarEstadoAP
*Nivel de acceso*: Solo usuarios auditores
*Requiere token:* Si
*Descripción:* Cambia el estado de una auditoria a "Aplicada", se debe de aplicar cuando se termina de contstar una auditoría (siguiente endpoint como referencia).
*Parámetros del body:* Objeto IdAuditoria con el id de la auditoría ya contestada.
![Endpoint para cambiar el estado a aplicada](/imagenes/16.png)
#### PUT
*Ruta de acceso:* /AplicarAuditoria
*Nivel de acceso*: Solo usuarios auditores
*Requiere token:* Si
*Descripción:* De la lista de preguntas relacionadas a una auditoria, les cambia su respuesta inicial (la cual era nula) a una respuesta del catálogo de las respuestas válidas.
*Parámetros del body:* Recibe una lista con las preguntas asociadas a una auditoría, las cuáles van a ser contestadas.
![Endpoint para contestar las preguntas de una auditoría](/imagenes/15.png)

### Diagrama de proceso de la API
![Diagrama de proceso](/imagenes/Proceso.png)

### Modelo relacional de la base de datos
![Modelo ER](/imagenes/base.png)
