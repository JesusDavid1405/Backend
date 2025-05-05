using System;
using Entity.Model;
using Entity.DTO;
using Microsoft.Extensions.Logging;
using Entity.context;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories;

public class AuthRepository
{
    private readonly ILogger<AuthRepository> _logger;  
    private readonly ApplicationDbContext _context;

    public AuthRepository(ILogger<AuthRepository> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.Password == password &&
                    u.Active);

            return user; // Si no lo encuentra, devuelve null
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar validar las credenciales para el email {email}", email);
            throw;
        }
    }

    public async Task<bool> Register(RegisterDTO registerDTO)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Crear persona primero
            var person = new Person
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                DocumentType = registerDTO.DocumentType,
                Document = registerDTO.Document,
                DateBorn = registerDTO.DateBorn,
                PhoneNumber = registerDTO.PhoneNumber,
                Eps = registerDTO.Eps,
                Genero = registerDTO.Genero,
                RelatedPerson = registerDTO.RelatedPerson
            };

            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();

            // 2. Crear usuario y enlazar con la persona
            var user = new User
            {
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Active = true,
                RegistrationDate = DateTime.Now,
                PersonId = person.Id // 👈 aquí va la FK
            };

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            // 3. Asignar roles (igual que antes)
            foreach (var roleName in registerDTO.Roles)
            {
                var role = await _context.Role.FirstOrDefaultAsync(r => r.Name == roleName);

                if (role != null)
                {
                    var rolUser = new RolUser
                    {
                        UserId = user.Id,
                        RolId = role.Id
                    };

                    await _context.RolUser.AddAsync(rolUser);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error durante el registro");
            return false;
        }
    }

    public async Task<RolUser> getRolUserWithId(int id)
    {
        var rolUser = await _context.RolUser
            .Where(r => !r.IsDeleted && r.User.Id == id)
            .Select(r => new RolUser
            {
                Id = r.Id,
                RolId = r.RolId,
                UserId = r.UserId,
                IsDeleted = r.IsDeleted
            })
            .FirstOrDefaultAsync();

        return rolUser;
    }


}
