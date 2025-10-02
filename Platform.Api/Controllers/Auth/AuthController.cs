using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Application.Core.Auth.Commands.Authentication;
using Platform.Application.Core.Auth.Queries.Handlers;
using Platform.Application.Utils;
using Platform.Domain.DTOs.Auth;

namespace Platform.Api.Controllers.Auth
{
    /// <summary>
    /// Controlador para la autenticación de usuarios.
    /// </summary>
    [ApiController]
    [Route("Api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ILoginCommand _loginCommand;
        private readonly IUserMeQueryHandler _userMeQueryHandler;

        /// <summary>
        /// Constructor de la clase AuthController.
        /// </summary>
        /// <param name="loginCommand">Comando para manejar el inicio de sesión.</param>
        /// <param name="logger">Instancia del logger para registrar información.</param>
        /// <param name="userMeQueryHandler">Handler para obtener información del usuario autenticado.</param>
        public AuthController(ILoginCommand loginCommand, ILogger<AuthController> logger, IUserMeQueryHandler userMeQueryHandler)
            => (_loginCommand, _logger, _userMeQueryHandler) = (loginCommand, logger, userMeQueryHandler);

        /// <summary>
        /// Inicia sesión en el sistema con las credenciales proporcionadas.
        /// </summary>
        /// <param name="autorizacion">Objeto que contiene las credenciales de inicio de sesión (email y contraseña).</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
        /// <returns>Un token JWT si las credenciales son válidas; de lo contrario, un error.</returns>
        /// <response code="200">Retorna el token JWT de autenticación</response>
        /// <response code="400">Credenciales inválidas o datos de entrada incorrectos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login");
            LoginResponse? Response = await _loginCommand.Login(autorizacion, cancellationToken);

            if (Response == null)
                return BadRequest();

            return Ok(Response.Token);
        }

        /// <summary>
        /// Obtiene la información del usuario autenticado en formato híbrido con navegación y permisos agrupados.
        /// Formato optimizado para frontends con navegación dinámica y permisos granulares.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
        /// <returns>Información completa del usuario autenticado incluyendo roles, permisos y navegación.</returns>
        /// <response code="200">Retorna la información del usuario autenticado</response>
        /// <response code="401">Usuario no autenticado o token inválido</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [Route("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserMeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserMeResponseDto>> GetMe(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetMe endpoint called");

                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(CustomClaimTypes.UserId)?.Value;
                _logger.LogInformation("User ID claim from token: {UserId}", userIdClaim);

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    _logger.LogWarning("Invalid or missing user ID in token");
                    return Unauthorized(new { message = "Invalid or missing user ID in token" });
                }

                _logger.LogInformation("Parsed user ID: {UserId}", userId);

                // Obtener la información del usuario en formato híbrido
                var userMeHybrid = await _userMeQueryHandler.GetUserMe(userId, cancellationToken);

                _logger.LogInformation("Successfully retrieved hybrid user info for user ID: {UserId}", userId);
                return Ok(userMeHybrid);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found in database");
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user information");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }
    }
}
