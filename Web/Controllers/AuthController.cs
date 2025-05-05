using Business;
using Business.Services;
using Entity.DTO;
using Entity.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Utilities;

namespace Web.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly Jwt _jwt;

    public AuthController(AuthService authService, ILogger<AuthController> logger, Jwt jwt)
    {
        _authService = authService;
        _logger = logger;
        _jwt = jwt;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _authService.Login(loginDTO);

            if (result == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var rol = await _authService.getRolUserWithId(result.Id);

            var token = _jwt.GenerarJwt(result, rol.RolId);

            return StatusCode(StatusCodes.Status200OK, new
            {
                isSuccess = true,
                token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el login.");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        try
        {
            if (registerDTO == null)
                return BadRequest(new { message = "Datos de registro inválidos." });

            // Validación básica opcional (ejemplo: campos obligatorios)
            if (string.IsNullOrWhiteSpace(registerDTO.Email) || string.IsNullOrWhiteSpace(registerDTO.Password))
                return BadRequest(new { message = "Email y contraseña son obligatorios." });

            var result = await _authService.Register(registerDTO);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar el usuario");
            return StatusCode(500, new { message = ex.Message });
        }
    }


}
