using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Mapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;

namespace FSI.AccessAuthentication.Application.Services
{
    public class UserAppService : BaseAppService<UserDto, UserEntity>, IUserAppService
    {
        public UserAppService(IUserRepository repository)
            : base(repository, new UserMapper()) { }
    }
}
