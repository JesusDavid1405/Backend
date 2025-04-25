using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Core;
using Entity.DTO;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IUserServices : IServiceBase<UserDTO, User>
    {
        //Task<bool> LoginAsync(UserDTO user);
    }
}
