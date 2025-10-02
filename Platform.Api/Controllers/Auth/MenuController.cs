using Platform.Application.Core.Auth.Commands.Handlers;
using Platform.Application.Core.Auth.Queries.Handlers;
using Platform.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.Attributes;

namespace Platform.Api.Controllers.Auth
{
    /// <summary>
    /// Controlador para gestionar menús del sistema.
    /// </summary>
    [Route("Api/Auth/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuCommandHandler _menuCommandHandler;
        private readonly IMenuQueryHandler _menuQueryHandler;

        public MenuController(
            IMenuCommandHandler menuCommandHandler,
            IMenuQueryHandler menuQueryHandler)
        {
            _menuCommandHandler = menuCommandHandler;
            _menuQueryHandler = menuQueryHandler;
        }

        /// <summary>
        /// Obtiene todos los menús del sistema
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de menús</returns>
        [HttpGet]
        [RequirePermission("menu.read")]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetAllMenus(CancellationToken cancellationToken)
        {
            try
            {
                var menus = await _menuQueryHandler.GetAllMenusAsync(cancellationToken);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la estructura jerárquica de menús
        /// </summary>
        /// <param name="activeOnly">Si solo se deben obtener menús activos</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Estructura de árbol de menús</returns>
        [HttpGet("tree")]
        [RequirePermission("menu.read")]
        public async Task<ActionResult<IEnumerable<MenuTreeDto>>> GetMenuTree(
            [FromQuery] bool activeOnly = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var menuTree = await _menuQueryHandler.GetMenuTreeAsync(activeOnly, cancellationToken);
                return Ok(menuTree);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un menú por su ID
        /// </summary>
        /// <param name="id">ID del menú</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Datos del menú</returns>
        [HttpGet("{id:guid}")]
        [RequirePermission("menu.read")]
        public async Task<ActionResult<MenuDto>> GetMenuById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var menu = await _menuQueryHandler.GetMenuByIdAsync(id, cancellationToken);
                if (menu == null)
                {
                    return NotFound(new { message = "Menú no encontrado" });
                }
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo menú
        /// </summary>
        /// <param name="createMenuDto">Datos del menú a crear</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Menú creado</returns>
        [HttpPost]
        [RequirePermission("menu.create")]
        public async Task<ActionResult<MenuDto>> CreateMenu(
            [FromBody] CreateMenuDto createMenuDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var menu = await _menuCommandHandler.CreateMenuAsync(createMenuDto, cancellationToken);
                return CreatedAtAction(nameof(GetMenuById), new { id = menu.Id }, menu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un menú existente
        /// </summary>
        /// <param name="id">ID del menú a actualizar</param>
        /// <param name="updateMenuDto">Datos actualizados del menú</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Menú actualizado</returns>
        [HttpPut("{id:guid}")]
        [RequirePermission("menu.update")]
        public async Task<ActionResult<MenuDto>> UpdateMenu(
            Guid id,
            [FromBody] UpdateMenuDto updateMenuDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var menu = await _menuCommandHandler.UpdateMenuAsync(id, updateMenuDto, cancellationToken);
                return Ok(menu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un menú
        /// </summary>
        /// <param name="id">ID del menú a eliminar</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id:guid}")]
        [RequirePermission("menu.delete")]
        public async Task<ActionResult> DeleteMenu(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _menuCommandHandler.DeleteMenuAsync(id, cancellationToken);
                if (!result)
                {
                    return NotFound(new { message = "Menú no encontrado" });
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }
    }
}
