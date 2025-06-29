using FSI.AccessAuthentication.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.AccessAuthentication.Application.Interfaces
{
    public interface IUserAppService : IBaseAppService<UserDto>
    {
        // Métodos específicos do usuário podem ser adicionados aqui futuramente
    }
}
