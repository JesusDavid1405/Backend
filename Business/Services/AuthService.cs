using System;
using Data.Repositories;
using Microsoft.Extensions.Logging;
using Entity.DTO;
using Entity.Model;
using Mapster;
using Utilities;


namespace Business.Services;

public class AuthService
{
    private readonly AuthRepository _AuthRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AuthRepository AuthRepository, ILogger<AuthService> logger)
    {
        _AuthRepository = AuthRepository;
        _logger = logger; 
    }

    async public Task<User> Login(LoginDTO loginDTO)
    {
        try
        {
            var exists = await _AuthRepository.LoginAsync(loginDTO.Email, loginDTO.Password);

            return exists.Adapt<User>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar las credenciales");
            throw;
        }
    }

    public async Task<RegisterDTO> Register(RegisterDTO registerDTO)
    {
        try
        {
            // Registrar usuario, persona y roles
            var registeredUser = await _AuthRepository.Register(registerDTO);

            // Mapear entidad a DTO si es necesario (puedes usar Mapster, Automapper, etc.)
            var result = new RegisterDTO
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                RegistrationDate = DateTime.Now
            };

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el registro de usuario");
            throw;
        }
    }

    public async Task<RolUserDTO> getRolUserWithId(int id)
    {
        try
        {
            var result = await _AuthRepository.getRolUserWithId(id);
            return result.Adapt<RolUserDTO>();
        }
        catch (Exception ex)
        {
            // Loguear o manejar el error adecuadamente
            throw new Exception("Error al obtener el rol del usuario", ex);
        }
    }
}
