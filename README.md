# DDD-NetCore-API

API RESTful principal de la solución Platform, diseñada con un enfoque en la escalabilidad, mantenibilidad y la separación de preocupaciones siguiendo los principios de **Domain-Driven Design (DDD)** y la **Arquitectura Limpia (Clean Architecture)**.

## 📋 Descripción

Este proyecto implementa una API robusta para gestión de autenticación y autorización, construida con **.NET 9.0** y siguiendo patrones de desarrollo empresarial. La arquitectura está diseñada para ser escalable, mantenible y testeable.

### Características principales

- 🏗️ **Arquitectura Limpia** con separación clara de responsabilidades
- 🎯 **Domain-Driven Design (DDD)** para organización por dominios
- ⚡ **CQRS Pattern** para separación de comandos y consultas
- 🔐 **Sistema de autenticación JWT** robusto
- 👥 **Gestión de roles y permisos** granulares
- 📊 **Entity Framework Core** con SQL Server
- 📝 **Documentación Swagger** integrada
- 🔄 **AutoMapper** para mapeo de objetos
- 🛡️ **CORS** configurado para múltiples entornos

## 🏛️ Arquitectura

El proyecto sigue el patrón de **Arquitectura Limpia** organizado en 4 capas:

```
DDD-NetCore-API/
├── Platform.Api/                 # 🌐 Capa de Presentación
│   ├── Controllers/              # Controladores API
│   ├── Extensions/               # Configuración de servicios
│   └── Attributes/               # Atributos de autorización
├── Platform.Application/         # 🎯 Capa de Aplicación
│   ├── Core/Auth/               # Lógica CQRS
│   │   ├── Commands/            # Comandos (escritura)
│   │   └── Queries/             # Consultas (lectura)
│   ├── Mappings/                # Perfiles de AutoMapper
│   └── Services/                # Servicios de aplicación
├── Platform.Domain/              # 🏛️ Capa de Dominio
│   ├── Entities/                # Entidades del dominio
│   ├── DTOs/                    # Objetos de transferencia
│   └── Repositories/            # Interfaces de repositorios
└── Platform.Infrastructure/      # 🔧 Capa de Infraestructura
    ├── DbContexts/              # Contexto de Entity Framework
    └── Repositories/            # Implementación de repositorios
```

### Flujo de dependencias

```
Platform.Api → Platform.Application → Platform.Domain
                        ↓
            Platform.Infrastructure → Platform.Domain
```

## 🛠️ Tecnologías

| Componente | Tecnología | Versión |
|------------|------------|---------|
| **Framework** | .NET Core | 9.0 |
| **Web API** | ASP.NET Core | 9.0 |
| **ORM** | Entity Framework Core | 9.0.9 |
| **Base de datos** | SQL Server | - |
| **Autenticación** | JWT Bearer | 9.0.9 |
| **Mapeo de objetos** | AutoMapper | 15.0.1 |
| **Hash de contraseñas** | BCrypt.Net-Next | 4.0.3 |
| **Documentación** | Swagger/OpenAPI | 9.0.5 |
| **Contenedores** | Docker | ✅ |

## 🚀 Instalación y Ejecución

### Prerrequisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB o instancia completa)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Entity Framework Core Tools](https://docs.microsoft.com/es-es/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### Recursos adicionales

- **Diagrama de la base de datos**: El archivo `Diagrama_platform.png` en la raíz del proyecto muestra la estructura de la base de datos y las relaciones entre entidades.
- **Script SQL**: El archivo `script_platform.sql` contiene el script completo para crear la base de datos y todas sus tablas manualmente si se prefiere este método en lugar de las migraciones.

### 1. Clonar el repositorio

```bash
git clone https://github.com/DidierAvila/DDD-NetCore-API.git
cd DDD-NetCore-API
```

### 2. Configurar la base de datos

Actualizar la cadena de conexión en `Platform.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Platform;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Restaurar paquetes

```bash
dotnet restore
```

### 4. Compilar la solución

```bash
dotnet build
```

### 5. Migraciones de base de datos

El proyecto incluye migraciones de Entity Framework Core para la creación y actualización de la base de datos:

- **InitialCreate**: Crea la estructura inicial de la base de datos con todas las tablas necesarias
- **SeedInitialData**: Agrega datos iniciales para las tablas principales (UserTypes, Roles, Permissions, Countries)

Las migraciones se aplican automáticamente al iniciar la aplicación en entorno de desarrollo gracias a la extensión `MigrateDatabase()` configurada en `Program.cs`.

Para aplicar las migraciones manualmente:

```bash
dotnet ef database update --project Platform.Infrastructure --startup-project Platform.Api
```

Para crear nuevas migraciones cuando se modifiquen las entidades:

```bash
dotnet ef migrations add NombreMigracion --project Platform.Infrastructure --startup-project Platform.Api
```

### 6. Ejecutar la aplicación

```bash
dotnet run --project Platform.Api
```

La API estará disponible en:
- **HTTPS**: https://localhost:7070
- **HTTP**: http://localhost:5070
- **Swagger**: https://localhost:7070/swagger

## 📚 Uso de la API

### Autenticación

1. **Login**: `POST /Api/Auth/Login`
```json
{
  "email": "usuario@ejemplo.com",
  "password": "contraseña"
}
```

2. **Obtener información del usuario**: `GET /Api/Auth/me`
```bash
Authorization: Bearer {token}
```

### Endpoints principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/Api/Auth/Login` | Iniciar sesión |
| `GET` | `/Api/Auth/me` | Información del usuario autenticado |
| `GET` | `/Api/Users` | Listar usuarios |
| `POST` | `/Api/Users` | Crear usuario |
| `PUT` | `/Api/Users/{id}` | Actualizar usuario |
| `DELETE` | `/Api/Users/{id}` | Eliminar usuario |
| `GET` | `/Api/Roles` | Listar roles |
| `GET` | `/Api/Permissions` | Listar permisos |

## 🔐 Configuración de Seguridad

### JWT Configuration

En `appsettings.json`:

```json
{
  "JwtSettings": {
    "key": "tu-clave-secreta-super-segura-de-al-menos-32-caracteres"
  }
}
```

### CORS Configuration

```json
{
  "CorsSettings": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "https://tu-frontend.com"
    ]
  }
}
```

## 🧪 Testing

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📦 Docker

### Construir imagen

```bash
docker build -t platform-api .
```

### Ejecutar contenedor

```bash
docker run -p 8080:80 platform-api
```

## 🏗️ Patrones Implementados

- **Clean Architecture**: Separación de responsabilidades en capas
- **Domain-Driven Design**: Organización por dominios de negocio
- **CQRS**: Separación de comandos (escritura) y consultas (lectura)
- **Repository Pattern**: Abstracción del acceso a datos
- **Dependency Injection**: Inversión de control integrada
- **DTO Pattern**: Objetos de transferencia optimizados
- **Database Migrations**: Gestión de cambios en la estructura de la base de datos
- **Seed Data**: Inicialización de datos para entornos de desarrollo

## 📝 Estructura del Dominio Auth

### Entidades principales

- **User**: Usuarios del sistema
- **Role**: Roles de usuario
- **Permission**: Permisos granulares
- **Menu**: Menús de navegación
- **Session**: Sesiones de usuario
- **UserType**: Tipos de usuario

### Casos de uso implementados

- ✅ Autenticación y autorización
- ✅ Gestión de usuarios
- ✅ Sistema de roles y permisos
- ✅ Menús dinámicos por permisos
- ✅ Configuraciones por tipo de usuario

## 🔄 Flujo CQRS

```
Controller → Command/Query → Handler → Repository → Database
     ↓              ↓           ↓          ↓
   DTO ←→ AutoMapper ←→ Entity ←→ DbContext
```

## 🚀 Roadmap

- [ ] **Testing**: Pruebas unitarias e integración
- [ ] **Logging**: Implementar Serilog estructurado
- [ ] **Caching**: Redis para optimización
- [ ] **Health Checks**: Monitoreo de salud
- [ ] **Metrics**: Telemetría y monitoreo
- [ ] **API Versioning**: Versionado de endpoints
- [ ] **Rate Limiting**: Control de velocidad
- [ ] **Background Jobs**: Procesamiento asíncrono

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit los cambios (`git commit -m 'Add: Amazing Feature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver `LICENSE` para más detalles.

## 👥 Equipo

- **Didier Avila** - [@DidierAvila](https://github.com/DidierAvila)

## 📞 Contacto

- **Email**: desarrollo@platform.com
- **Proyecto**: [DDD-NetCore-API](https://github.com/DidierAvila/DDD-NetCore-API)

---

⭐ **¡Dale una estrella si te gustó el proyecto!**