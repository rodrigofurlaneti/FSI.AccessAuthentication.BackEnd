using FSI.AccessAuthentication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.AccessAuthentication.Domain.Interfaces
{
    public interface IAuthenticationRepository : IBaseRepository<AuthenticationEntity>
    {
        Task<AuthenticationEntity> GetByAuthenticationAsync(AuthenticationEntity authenticationEntity);
    }
}
