using FSI.AccessAuthentication.Domain.Entities;

namespace FSI.AccessAuthentication.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity?> GetByUsernameAsync(string username);
        Task<UserEntity?> GetByPasswordAsync(string password);
    }
}
