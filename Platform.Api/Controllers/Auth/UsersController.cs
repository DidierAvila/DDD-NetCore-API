using Platform.Application.Core.Auth.Commands.Handlers;
using Platform.Application.Core.Auth.Commands.Users;
using Platform.Application.Core.Auth.Queries.Handlers;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.Attributes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Platform.Api.Controllers.Auth
{
    /// <summary>
    /// Controlador para gestionar los usuarios del sistema.
    /// </summary>
    [ApiController]
    [Route("Api/Auth/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserCommandHandler _userCommandHandler;
        private readonly IUserQueryHandler _userQueryHandler;

        /// <summary>
        /// Constructor del controlador UsersController
        /// </summary>
        /// <param name="userCommandHandler"></param>
        /// <param name="userQueryHandler"></param>
        /// <param name="logger"></param>
        public UsersController(IUserCommandHandler userCommandHandler, IUserQueryHandler userQueryHandler, ILogger<UsersController> logger)
        {
            _userCommandHandler = userCommandHandler;
            _userQueryHandler = userQueryHandler;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista paginada de usuarios con filtros opcionales
        /// </summary>
        /// <param name="filter">Filtros de búsqueda y paginación</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista paginada de usuarios</returns>
        /// <remarks>
        /// Campos disponibles para SortBy: name, email, username, createdat, usertypeid
        /// 
        /// Ejemplo de uso:
        /// GET /api/users?page=1&amp;pageSize=10&amp;search=juan&amp;sortBy=name
        /// </remarks>
        [HttpGet]
        [RequirePermission("users.read")]
        public async Task<ActionResult<PaginationResponseDto<UserBasicDto>>> GetAll(
            [FromQuery] UserFilterDto filter, 
            CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userQueryHandler.GetAllUsersFiltered(filter, cancellationToken);
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users with filters");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Obtiene una lista de usuarios para dropdown filtrados por tipo de usuario
        /// </summary>
        /// <param name="userTypeId">ID del tipo de usuario</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de usuarios activos del tipo especificado</returns>
        [HttpGet("dropdown/bytype/{userTypeId}")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<IEnumerable<UserDropdownDto>>> GetUsersByTypeForDropdown([FromRoute] Guid userTypeId, CancellationToken cancellationToken)
        {
            var users = await _userQueryHandler.GetUsersByTypeForDropdown(userTypeId, cancellationToken);
            return Ok(users);
        }
        
        /// <summary>
        /// Obtiene una lista de usuarios tipo Supplier para dropdown
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de usuarios Supplier activos</returns>
        [HttpGet("dropdown/suppliers")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<IEnumerable<UserDropdownDto>>> GetSupplierUsersForDropdown(CancellationToken cancellationToken)
        {
            var suppliers = await _userQueryHandler.GetSupplierUsersForDropdown(cancellationToken);
            return Ok(suppliers);
        }

        /// <summary>
        /// Obtiene un usuario por su ID incluyendo sus roles asignados
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Usuario con roles asignados</returns>
        /// <remarks>
        /// Respuesta incluye:
        /// - Información básica del usuario
        /// - Tipo de usuario (UserType)
        /// - Lista completa de roles asignados con detalles
        /// 
        /// Ejemplo de respuesta:
        /// {
        ///   "id": "uuid-usuario",
        ///   "name": "Juan Pérez",
        ///   "email": "juan@empresa.com",
        ///   "userTypeName": "Empleado",
        ///   "roles": [
        ///     {
        ///       "id": "uuid-rol",
        ///       "name": "Administrador",
        ///       "description": "Acceso completo"
        ///     }
        ///   ]
        /// }
        /// </remarks>
        [HttpGet("{id}")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userQueryHandler.GetUserById(id, cancellationToken);
                if (user == null)
                    return NotFound($"User with ID {id} not found");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un nuevo usuario con roles opcionales
        /// </summary>
        /// <param name="createDto">Datos del usuario a crear, incluyendo roles opcionales</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Usuario creado con roles asignados</returns>
        /// <remarks>
        /// Ejemplo de request body:
        /// {
        ///   "name": "Juan Pérez",
        ///   "email": "juan.perez@empresa.com",
        ///   "password": "Password123!",
        ///   "userTypeId": "uuid-del-tipo-usuario",
        ///   "roleIds": ["uuid-rol-1", "uuid-rol-2"]
        /// }
        /// 
        /// Si no se especifican roles (roleIds), el usuario se creará sin roles asignados.
        /// Los roles se pueden asignar posteriormente usando los endpoints de gestión de roles.
        /// </remarks>
        [HttpPost]
        [RequirePermission("users.create")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userCommandHandler.CreateUser(createDto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="updateDto">Datos a actualizar del usuario</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Usuario actualizado con roles asignados</returns>
        /// <remarks>
        /// La respuesta incluye los roles actuales del usuario después de la actualización.
        /// Para gestionar roles específicamente, usar los endpoints de gestión de roles:
        /// - POST /api/auth/users/{id}/roles - Asignar roles
        /// - DELETE /api/auth/users/{id}/roles - Remover roles
        /// 
        /// Ejemplo de request:
        /// {
        ///   "name": "Juan Pérez Actualizado",
        ///   "phone": "555-9999",
        ///   "additionalData": {
        ///     "position": "Senior Developer"
        ///   }
        /// }
        /// 
        /// Respuesta incluye roles actuales:
        /// {
        ///   "id": "uuid-usuario",
        ///   "name": "Juan Pérez Actualizado",
        ///   "roles": [...roles actuales...]
        /// }
        /// </remarks>
        [HttpPut("{id}")]
        [RequirePermission("users.update")]
        public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UpdateUserDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userCommandHandler.UpdateUser(id, updateDto, cancellationToken);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un usuario por su ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [RequirePermission("users.delete")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userCommandHandler.DeleteUser(id, cancellationToken);
                if (result)
                    return NoContent();
                
                return BadRequest("Failed to delete user");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changePasswordDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}/change-password")]
        [RequirePermission("users.update")]
        public async Task<ActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userCommandHandler.ChangePassword(id, changePasswordDto, cancellationToken);
                if (result)
                    return NoContent();
                
                return BadRequest("Failed to change password");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user with ID {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Establece un valor en los datos adicionales del usuario
        /// </summary>
        [HttpPut("{id}/additional-data/{key}")]
        [RequirePermission("users.update")]
        public async Task<ActionResult<UserAdditionalValueResponseDto>> SetAdditionalValue(
            Guid id, 
            string key, 
            [FromBody] object value, 
            CancellationToken cancellationToken)
        {
            try
            {
                var operationDto = new UserAdditionalDataOperationDto { Key = key, Value = value };
                var result = await _userCommandHandler.SetUserAdditionalValue(id, operationDto, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting additional value for user {UserId}, key {Key}", id, key);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un valor específico de los datos adicionales del usuario
        /// </summary>
        [HttpGet("{id}/additional-data/{key}")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<UserAdditionalValueResponseDto>> GetAdditionalValue(
            Guid id, 
            string key, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userCommandHandler.GetUserAdditionalValue(id, key, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting additional value for user {UserId}, key {Key}", id, key);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un valor de los datos adicionales del usuario
        /// </summary>
        [HttpDelete("{id}/additional-data/{key}")]
        [RequirePermission("users.update")]
        public async Task<ActionResult<bool>> RemoveAdditionalValue(
            Guid id, 
            string key, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userCommandHandler.RemoveUserAdditionalValue(id, key, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing additional value for user {UserId}, key {Key}", id, key);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza todos los datos adicionales del usuario
        /// </summary>
        [HttpPut("{id}/additional-data")]
        [RequirePermission("users.update")]
        public async Task<ActionResult<Dictionary<string, object>>> UpdateAdditionalData(
            Guid id, 
            [FromBody] UpdateUserAdditionalDataDto updateDto, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userCommandHandler.UpdateUserAdditionalData(id, updateDto, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating additional data for user {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ======================================
        // ROLE MANAGEMENT ENDPOINTS
        // ======================================

        /// <summary>
        /// Obtiene todos los roles asignados a un usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de roles del usuario</returns>
        [HttpGet("{id}/roles")]
        [RequirePermission("users.read")]
        public async Task<ActionResult<List<UserRoleDto>>> GetUserRoles(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _userCommandHandler.GetUserRoles(id, cancellationToken);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Asigna múltiples roles a un usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="roleIds">Lista de IDs de roles a asignar</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Resultado de la asignación múltiple</returns>
        [HttpPost("{id}/roles")]
        [RequirePermission("users.manage")]
        public async Task<ActionResult<MultipleRoleAssignmentResult>> AssignRolesToUser(
            Guid id, 
            [FromBody] List<Guid> roleIds, 
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new AssignMultipleRolesToUser
                {
                    UserId = id,
                    RoleIds = roleIds
                };

                var result = await _userCommandHandler.AssignMultipleRolesToUser(command, cancellationToken);
                
                if (result.IsFullySuccessful)
                {
                    return Ok(result);
                }
                else if (result.AssignedRoles.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning roles to user {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Remueve múltiples roles de un usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="roleIds">Lista de IDs de roles a remover</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Resultado de la remoción múltiple</returns>
        [HttpDelete("{id}/roles")]
        [RequirePermission("users.manage")]
        public async Task<ActionResult<MultipleRoleRemovalResult>> RemoveRolesFromUser(
            Guid id, 
            [FromBody] List<Guid> roleIds, 
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new RemoveMultipleRolesFromUser
                {
                    UserId = id,
                    RoleIds = roleIds
                };

                var result = await _userCommandHandler.RemoveMultipleRolesFromUser(command, cancellationToken);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing roles from user {UserId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
