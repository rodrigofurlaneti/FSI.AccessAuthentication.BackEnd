using FSI.AccessAuthentication.Domain.Entities;

namespace FSI.AccessAuthentication.Domain.Interfaces
{
    public interface IAuthenticationRepository : IBaseRepository<AuthenticationEntity>
    {
        Task<AuthenticationEntity> GetByAuthenticationAsync(AuthenticationRequestEntity authenticationEntity);
    }
}
