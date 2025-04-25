using Business.Core;
using Business.Interfaces;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UserService : ServiceBase<UserDTO, User>, IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
            : base(userRepository, logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        //public async Task<bool> LoginAsync(UserDTO userDTO)
        //{
        //    try
        //    {
        //        var result = await _userRepository.Login(userDTO.Email, userDTO.Password);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al intentar iniciar sesión del usuario con email: {Email}", userDTO.Email);
        //        return false;
        //    }
        //}
    }
}
                                                                                    